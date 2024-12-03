using CommitLint.Net.Models;
using CommitLint.Net.Rules;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.RulesTests;

public class MaxSubjectLengthRuleTests
{
    [TestCase("fix: 1", 5)]
    [TestCase("feat: description", 10)]
    [TestCase("feat: description", 16)]
    [TestCase(
        "feat: this is a feature commit that adds some functionality and has subject line that is longer than max allowed",
        90
    )]
    [TestCase(
        "feat: this is a feature commit that adds some functionality and has subject line that is longer than max allowed",
        111
    )]
    public void WhenSubjectIsLongerThanMaxAllowed_AndRuleIsEnabled_ThenRuleIsNotValid(
        string message,
        int maxLength
    )
    {
        // Arrange
        var config = new MaxSubjectLength { Enabled = true, Value = maxLength };
        var rule = new MaxSubjectLengthRule(config);

        // Act
        var result = rule.IsValid(new[] { message });

        // Assert
        result.IsValid.Should().BeFalse();
        result.Message.Should().NotBeEmpty();
    }

    [TestCase("fix: 1", 6)]
    [TestCase("feat: description", 17)]
    [TestCase("feat: description", 18)]
    [TestCase(
        "feat: this is a feature commit that adds some functionality and has subject line that is longer than max allowed",
        112
    )]
    [TestCase(
        "feat: this is a feature commit that adds some functionality and has subject line that is longer than max allowed",
        113
    )]
    public void WhenSubjectIsNotLongerThanMaxAllowed_AndRuleIsEnabled_ThenRuleIsValid(
        string message,
        int maxLength
    )
    {
        // Arrange
        var config = new MaxSubjectLength { Enabled = true, Value = maxLength };
        var rule = new MaxSubjectLengthRule(config);

        // Act
        var result = rule.IsValid(new[] { message });

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void WhenSubjectIsLongerThanMaxAllowed_AndRuleIsDisabled_ThenRuleIsValid()
    {
        // Arrange
        var config = new MaxSubjectLength { Enabled = false, Value = 1 };
        var rule = new MaxSubjectLengthRule(config);
        string[] commitMessageLines = new[] { "feat: some description" };

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
