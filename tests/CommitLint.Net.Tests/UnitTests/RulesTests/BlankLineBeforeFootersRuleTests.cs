using CommitLint.Net.Models;
using CommitLint.Net.Rules;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.RulesTests
{
    public class BlankLineBeforeFootersRuleTests
    {
        [Test]
        public void WhenNoFootersExist_ThenRuleIsValid()
        {
            // Arrange
            var config = new ConventionalCommitConfig { Enabled = true };
            var rule = new BlankLineBeforeFootersRule(config);
            string[] commitMessageLines = ["fix: some bug fix"];

            // Act
            var result = rule.IsValid(commitMessageLines);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [TestCase("fix: some bug fix", "", "not footer: value")]
        [TestCase("fix: some bug fix", "", "also not footer #123")]
        public void WhenSeparatorIsPrecededByMoreThan1Word_ThenLineIsNotTreatedAsFooter(
            params string[] commitMessageLines
        )
        {
            // Arrange
            var config = new ConventionalCommitConfig { Enabled = true };
            var rule = new BlankLineBeforeFootersRule(config);

            // Act
            var result = rule.IsValid(commitMessageLines);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [TestCase("fix: some bug fix", "Some-Footer: value")]
        [TestCase("fix: some bug fix", "Closes #123")]
        [TestCase("fix: some bug fix", "BREAKING CHANGE: some breaking change")]
        [TestCase("fix: some bug fix", "BREAKING-CHANGE: some breaking change")]
        public void WhenOneFooterExists_AndNotPrecededByBlankLine_ThenRuleIsNotValid(
            params string[] commitMessageLines
        )
        {
            // Arrange
            var config = new ConventionalCommitConfig { Enabled = true };
            var rule = new BlankLineBeforeFootersRule(config);

            // Act
            var result = rule.IsValid(commitMessageLines);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Message.Should().NotBeEmpty();
        }

        [TestCase("fix: some bug fix", "", "Footer: value")]
        [TestCase("fix: some bug fix", "", "", "", "Some-Footer: value")]
        [TestCase("fix: some bug fix", "", "Fixes #123")]
        [TestCase("fix: some bug fix", "", "", "Fixes #123")]
        [TestCase("fix: some bug fix", "", "BREAKING CHANGE: some breaking change")]
        [TestCase("fix: some bug fix", "", "BREAKING-CHANGE: some breaking change")]
        public void WhenOneFooterExists_AndPrecededByBlankLine_ThenRuleIsValid(
            params string[] commitMessageLines
        )
        {
            // Arrange
            var config = new ConventionalCommitConfig { Enabled = true };
            var rule = new BlankLineBeforeFootersRule(config);

            // Act
            var result = rule.IsValid(commitMessageLines);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [TestCase("fix: some bug fix", "", "Footer1: value1", "Footer2: value2")]
        [TestCase(
            "fix: some bug fix",
            "",
            "BREAKING CHANGE: some breaking change",
            "Footer2: value1"
        )]
        [TestCase(
            "fix: some bug fix",
            "",
            "Footer1: value1",
            "BREAKING-CHANGE: some breaking change"
        )]
        [TestCase("fix: some bug fix", "", "Closes #123", "Footer2: value1")]
        public void WhenMultipleFootersExist_AndFirstOneIsPrecededByBlankLine_ThenRuleIsValid(
            params string[] commitMessageLines
        )
        {
            // Arrange
            var config = new ConventionalCommitConfig { Enabled = true };
            var rule = new BlankLineBeforeFootersRule(config);

            // Act
            var result = rule.IsValid(commitMessageLines);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [TestCase("fix: some bug fix", "Footer1: value1", "", "Footer2: value2")]
        [TestCase("fix: some bug fix", "Closes #4343", "", "Footer2: value2")]
        [TestCase(
            "fix: some bug fix",
            "BREAKING CHANGE: some breaking change",
            "",
            "Footer2: value1"
        )]
        [TestCase(
            "fix: some bug fix",
            "Footer1: value1",
            "",
            "BREAKING-CHANGE: some breaking change"
        )]
        public void WhenMultipleFootersExist_AndFirstOneIsNotPrecededByBlankLine_ThenRuleIsNotValid(
            params string[] commitMessageLines
        )
        {
            // Arrange
            var config = new ConventionalCommitConfig { Enabled = true };
            var rule = new BlankLineBeforeFootersRule(config);

            // Act
            var result = rule.IsValid(commitMessageLines);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Message.Should().NotBeEmpty();
        }
    }
}
