using CommitLint.Net.Models;
using CommitLint.Net.Rules;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.RulesTests;

public class BodyNotEmptyRuleTests
{
    [Test]
    public void WhenBodyIsEmpty_AndRuleIsEnabled_ThenRuleIsNotValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = true };
        var rule = new BodyNotEmptyRule(config);
        string[] commitMessageLines = ["fix: update feature", "", ""];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Message.Should().NotBeEmpty();
    }

    [Test]
    public void WhenBodyIsEmpty_AndRuleIsDisabled_ThenRuleIsValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = false };
        var rule = new BodyNotEmptyRule(config);
        string[] commitMessageLines = ["fix: update feature", "", ""];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
