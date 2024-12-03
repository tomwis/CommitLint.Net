using CommitLint.Net.Models;
using CommitLint.Net.Validators;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.ValidatorsTests
{
    [TestFixture]
    public class CommitMessageValidatorTests
    {
        [TestCaseSource(nameof(ValidCommitMessages))]
        public void WhenCommitMessageIsValid_ThenReturnValidResult(string[] commitMessageLines)
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

        [Test]
        public void WhenFirstFooterDoesNotHaveBlankLine_ThenReturnInvalidResult()
        {
            // Arrange
            var subject = new CommitMessageValidator(GetConfig());
            string[] commitMessageLines =
            [
                "feat: description",
                "",
                "body of valid commit message",
                "footer: without colon",
            ];

            // Act
            var result = subject.Validate(commitMessageLines);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        [Test]
        public void WhenFooterIsEmpty_ThenReturnInvalidResult()
        {
            // Arrange
            var subject = new CommitMessageValidator(GetConfig());
            string[] commitMessageLines = ["feat: description", "", "empty-footer: "];

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

        private static IEnumerable<string[]> ValidCommitMessages()
        {
            yield return ["feat: valid commit message"];
            yield return ["fix: valid fix commit message"];
            yield return ["docs: readme update", "", "body of valid commit message"];
            yield return ["feat(scope): some update"];
            yield return ["feat(scope)!: some update"];
            yield return ["feat!: some update"];
            yield return
            [
                "docs: readme update",
                "",
                "body of valid commit message",
                "",
                "BREAKING CHANGE: some breaking change",
            ];
            yield return ["docs: readme update", "", "BREAKING-CHANGE: other breaking change"];
            yield return
            [
                "docs: readme update",
                "",
                "body of valid commit message",
                "",
                "Footer: some info",
            ];
            yield return ["docs: readme update", "", "Closes #124"];
            yield return
            [
                "feat: description",
                "",
                "body of valid commit message",
                "",
                "Closes #124",
            ];
            yield return ["feat: description", "", "Footer1: some info", "Footer2: some info"];
            yield return
            [
                "feat: description",
                "",
                "body of valid commit message",
                "",
                "Signed-off-by: Author <author@example.com>",
                "",
                "Co-authored-by: Contributor <contributor@example.com>",
            ];
        }
    }
}
