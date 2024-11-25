using CommitLint.Net.Models;
using CommitLint.Net.Validators;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.ValidatorsTests
{
    [TestFixture]
    public class CommitMessageValidatorTests
    {
        [TestCase("feat: valid commit message")]
        [TestCase("fix: valid fix commit message")]
        [TestCase("docs: readme update", "", "body of valid commit message")]
        [TestCase("feat(scope): some update")]
        [TestCase("feat(scope)!: some update")]
        [TestCase("feat!: some update")]
        [TestCase(
            "docs: readme update",
            "",
            "body of valid commit message",
            "",
            "BREAKING CHANGE: some breaking change"
        )]
        [TestCase("docs: readme update", "", "BREAKING CHANGE: other breaking change")]
        public void WhenCommitMessageIsValid_ThenReturnValidResult(
            params string[] commitMessageLines
        )
        {
            // Arrange
            var subject = new CommitMessageValidator(GetConfig());

            // Act
            var result = subject.Validate(commitMessageLines);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void WhenCommitMessageIsEmpty_ThenReturnInvalidResult()
        {
            // Arrange
            var subject = new CommitMessageValidator(GetConfig());
            string[] commitMessageLines = [string.Empty];

            // Act
            var result = subject.Validate(commitMessageLines);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        [Test]
        public void WhenSubjectExceedsMaxLength_ThenReturnInvalidResult()
        {
            // Arrange
            var config = GetConfig(maxSubjectLength: 10);
            var subject = new CommitMessageValidator(config);
            string[] commitMessageLines = ["feat: description"];

            // Act
            var result = subject.Validate(commitMessageLines);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        [Test]
        public void WhenCommitMessageDoesNotContainBlankLineBeforeBody_ThenReturnInvalidResult()
        {
            // Arrange
            var subject = new CommitMessageValidator(GetConfig());
            string[] commitMessageLines = ["feat: description", "Body"];

            // Act
            var result = subject.Validate(commitMessageLines);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        [Test]
        public void WhenCommitMessageBodyIsEmpty_ThenReturnInvalidResult()
        {
            // Arrange
            var subject = new CommitMessageValidator(GetConfig());
            string[] commitMessageLines = ["feat: This is a valid message", "", ""];

            // Act
            var result = subject.Validate(commitMessageLines);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        [Test]
        public void WhenCommitMessageTypeIsInvalid_ThenReturnInvalidResult()
        {
            // Arrange
            var subject = new CommitMessageValidator(GetConfig());
            string[] commitMessageLines = ["invalid_type: description"];

            // Act
            var result = subject.Validate(commitMessageLines);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        [Test]
        public void WhenCommitMessageScopeIsInvalid_ThenReturnInvalidResult()
        {
            // Arrange
            var subject = new CommitMessageValidator(GetConfig());
            string[] commitMessageLines = ["feat(invalid scope): description"];

            // Act
            var result = subject.Validate(commitMessageLines);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        [Test]
        public void WhenTypeAndScopePrefixHasInvalidBreakingChangeMarker_ThenReturnInvalidResult()
        {
            // Arrange
            var subject = new CommitMessageValidator(GetConfig());
            string[] commitMessageLines = ["feat!(scope): description"];

            // Act
            var result = subject.Validate(commitMessageLines);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        [Test]
        public void WhenBreakingChangeTokenIsNotUppercase_ThenReturnInvalidResult()
        {
            // Arrange
            var subject = new CommitMessageValidator(GetConfig());
            string[] commitMessageLines =
            [
                "feat: description",
                "",
                "breaking change: some breaking change",
            ];

            // Act
            var result = subject.Validate(commitMessageLines);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        private static CommitMessageConfig GetConfig(int? maxSubjectLength = null)
        {
            return new CommitMessageConfig
            {
                MaxSubjectLength = new MaxSubjectLength
                {
                    Enabled = true,
                    Value = maxSubjectLength ?? 50,
                },
                ConventionalCommit = new ConventionalCommitConfig
                {
                    Enabled = true,
                    Types = ["feat", "fix", "docs"],
                },
            };
        }
    }
}
