using CommitLint.Net.Models;
using CommitLint.Net.Rules;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.RulesTests;

public class BreakingChangeSubjectMarkerRuleTests
{
    [TestCase("feat!: description")]
    [TestCase("fix(scope)!: description")]
    public void WhenTypeAndScopePrefixHasSingleExclamationMarkAtTheEnd_AndRuleIsEnabled_ThenRuleIsValid(
        string commitMessage
    )
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = true };
        var rule = new BreakingChangeSubjectMarkerRule(config);

        // Act
        var result = rule.IsValid([commitMessage]);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [TestCase("feat(scope)!!: description")]
    [TestCase("feat!(scope)!: description")]
    [TestCase("fix!!(scope): description")]
    [TestCase("fix!(scope!): description")]
    [TestCase("!fix!(scope)!: description")]
    public void WhenTypeAndScopePrefixHasMultipleExclamationMarks_AndRuleIsEnabled_ThenRuleIsNotValid(
        string commitMessage
    )
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = true };
        var rule = new BreakingChangeSubjectMarkerRule(config);

        // Act
        var result = rule.IsValid([commitMessage]);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Message.Should().NotBeEmpty();
    }

    [TestCase("feat!(scope): description")]
    [TestCase("!fix(scope): description")]
    [TestCase("fix(scope!): description")]
    public void WhenTypeAndScopePrefixHasSingleExclamationMarkButNotAtTheEnd_AndRuleIsEnabled_ThenRuleIsNotValid(
        string commitMessage
    )
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = true };
        var rule = new BreakingChangeSubjectMarkerRule(config);

        // Act
        var result = rule.IsValid([commitMessage]);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Message.Should().NotBeEmpty();
    }

    [TestCase("feat(scope): description")]
    [TestCase("fix(scope): description")]
    public void WhenTypeAndScopePrefixHasNoExclamationMark_AndRuleIsEnabled_ThenRuleIsValid(
        string commitMessage
    )
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = true };
        var rule = new BreakingChangeSubjectMarkerRule(config);

        // Act
        var result = rule.IsValid(new[] { commitMessage });

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
