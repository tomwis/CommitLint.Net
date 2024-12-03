using CommitLint.Net.Models;
using CommitLint.Net.Rules;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.RulesTests;

public class DescriptionNotEmptyRuleTests
{
    [Test]
    public void WhenDescriptionIsEmpty_AndRuleIsEnabled_ThenRuleIsNotValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = true };
        var rule = new DescriptionNotEmptyRule(config);
        string[] commitMessageLines = ["feat: "];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Message.Should().NotBeEmpty();
    }

    [Test]
    public void WhenDescriptionIsEmpty_AndRuleIsDisabled_ThenRuleIsValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = false };
        var rule = new DescriptionNotEmptyRule(config);
        string[] commitMessageLines = ["feat: "];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
