using CommitLint.Net.Models;
using CommitLint.Net.Rules;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.RulesTests
{
    public class BreakingChangeTokenInFooterRuleTests
    {
        [TestCase("BREAKING CHANGE")]
        [TestCase("BREAKING-CHANGE")]
        public void WhenBreakingChangeFooterHasValidToken_ThenRuleIsValid(
            string breakingChangeToken
        )
        {
            // Arrange
            var config = new ConventionalCommitConfig { Enabled = true };
            var rule = new BreakingChangeTokenInFooterRule(config);
            string[] commitMessageLines = new[]
            {
                "feat: description",
                "",
                $"{breakingChangeToken}: some breaking change",
            };

            // Act
            var result = rule.IsValid(commitMessageLines);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [TestCase("feat: description")]
        [TestCase("feat: description", "", "This is the body of the commit message.")]
        public void WhenBreakingChangeTokenIsAbsent_ThenRuleIsValid(
            params string[] commitMessageLines
        )
        {
            // Arrange
            var config = new ConventionalCommitConfig { Enabled = true };
            var rule = new BreakingChangeTokenInFooterRule(config);

            // Act
            var result = rule.IsValid(commitMessageLines);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [TestCase("breaking change")]
        [TestCase("Breaking Change")]
        [TestCase("Breaking change")]
        [TestCase("bReAkInG cHaNgE")]
        [TestCase("breaking-change")]
        [TestCase("Breaking-Change")]
        [TestCase("Breaking-change")]
        [TestCase("bReAkInG-cHaNgE")]
        public void WhenBreakingChangeFooterTokenIsNotUppercase_ThenRuleIsNotValid(
            string breakingChangeToken
        )
        {
            // Arrange
            var config = new ConventionalCommitConfig { Enabled = true };
            var rule = new BreakingChangeTokenInFooterRule(config);
            string[] commitMessageLines = new[]
            {
                "feat: description",
                "",
                $"{breakingChangeToken}: some breaking change",
            };

            // Act
            var result = rule.IsValid(commitMessageLines);

            // Assert
            result.IsValid.Should().BeFalse();
        }
    }
}
