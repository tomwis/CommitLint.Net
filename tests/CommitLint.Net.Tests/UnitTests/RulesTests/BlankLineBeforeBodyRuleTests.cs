using CommitLint.Net.Models;
using CommitLint.Net.Rules;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.RulesTests;

public class BlankLineBeforeBodyRuleTests
{
    [Test]
    public void WhenThereIsNoBlankLineBeforeBody_AndRuleIsEnabled_ThenRuleIsNotValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = true };
        var rule = new BlankLineBeforeBodyRule(config);
        string[] commitMessage = ["feat: description", "Body"];

        // Act
        var result = rule.IsValid(commitMessage);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Message.Should().NotBeEmpty();
    }

    [Test]
    public void WhenThereIsNoBlankLineBeforeBody_AndRuleIsDisabled_ThenRuleIsValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = false };
        var rule = new BlankLineBeforeBodyRule(config);
        string[] commitMessage = ["feat: description", "Body"];

        // Act
        var result = rule.IsValid(commitMessage);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void WhenThereIsBlankLineBeforeBody_AndRuleIsEnabled_ThenRuleIsValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = true };
        var rule = new BlankLineBeforeBodyRule(config);
        string[] commitMessage = ["feat: description", "", "Body"];

        // Act
        var result = rule.IsValid(commitMessage);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
