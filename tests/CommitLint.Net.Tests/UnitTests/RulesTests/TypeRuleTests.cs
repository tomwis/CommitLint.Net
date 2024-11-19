using CommitLint.Net.Models;
using CommitLint.Net.Rules;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.RulesTests;

public class TypeRuleTests
{
    [TestCase("feat")]
    [TestCase("fix")]
    [TestCase("refactor")]
    public void WhenCommitMessageStartsWithAllowedType_AndRuleIsEnabled_ThenRuleIsValid(string type)
    {
        // Arrange
        var config = new ConventionalCommitConfig
        {
            Enabled = true,
            Types = ["feat", "fix", "refactor"],
        };
        var rule = new TypeRule(config);
        var commitMessageLines = new[] { $"{type}: description" };

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [TestCase("some")]
    [TestCase("wrong")]
    [TestCase("types")]
    public void WhenCommitMessageStartsWithNotAllowedType_AndRuleIsEnabled_ThenRuleIsNotValid(
        string type
    )
    {
        // Arrange
        var config = new ConventionalCommitConfig
        {
            Enabled = true,
            Types = ["feat", "fix", "refactor"],
        };
        var rule = new TypeRule(config);
        string[] commitMessageLines = [$"{type}: description"];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Message.Should().NotBeEmpty();
    }

    [TestCase("some")]
    [TestCase("wrong")]
    [TestCase("types")]
    public void WhenCommitMessageStartsWithNotAllowedType_AndRuleIsDisabled_ThenRuleIsValid(
        string type
    )
    {
        // Arrange
        var config = new ConventionalCommitConfig
        {
            Enabled = false,
            Types = ["feat", "fix", "refactor"],
        };
        var rule = new TypeRule(config);
        string[] commitMessageLines = [$"{type}: description"];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [TestCase(":")]
    [TestCase(" ")]
    public void WhenTypeIsNotFollowedByColonAndSpace_AndRuleIsEnabled_ThenRuleIsNotValid(
        string separator
    )
    {
        // Arrange
        var config = new ConventionalCommitConfig
        {
            Enabled = true,
            Types = ["feat", "fix", "refactor"],
        };
        var rule = new TypeRule(config);
        string[] commitMessageLines = [$"feat{separator}description"];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Message.Should().NotBeEmpty();
    }

    [TestCase(":")]
    [TestCase(" ")]
    public void WhenTypeIsNotFollowedByColonAndSpace_AndRuleIsDisabled_ThenRuleIsValid(
        string separator
    )
    {
        // Arrange
        var config = new ConventionalCommitConfig
        {
            Enabled = false,
            Types = ["feat", "fix", "refactor"],
        };
        var rule = new TypeRule(config);
        string[] commitMessageLines = [$"feat{separator}description"];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void WhenAllowedTypesAreNull_AndRuleIsEnabled_ThenRuleIsValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = true, Types = null };
        var rule = new TypeRule(config);
        string[] commitMessageLines = ["feat: description"];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void WhenAllowedTypesAreEmpty_AndRuleIsEnabled_ThenRuleIsNotValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = true, Types = [] };
        var rule = new TypeRule(config);
        string[] commitMessageLines = ["feat: description"];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
