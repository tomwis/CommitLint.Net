using CommitLint.Net.Models;
using CommitLint.Net.Rules;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.RulesTests;

public class ScopeRuleTests
{
    [TestCase("feat(scope): description")]
    [TestCase("fix(git): description")]
    [TestCase("refactor(other): description")]
    public void WhenCommitMessageHasValidScope_AndRuleIsEnabled_ThenRuleIsValid(
        string commitMessage
    )
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = true };
        var rule = new ScopeRule(config);

        // Act
        var result = rule.IsValid(new[] { commitMessage });

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void WhenCommitMessageDoesNotContainScope_AndRuleIsEnabled_ThenRuleIsValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = true };
        var rule = new ScopeRule(config);
        string[] commitMessageLines = new[] { "feat: description" };

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [TestCase("feat)(scope: description")]
    [TestCase("(feat)scope: description")]
    [TestCase("feat(invalid scope): description")]
    [TestCase("feat(invalid-scope): description")]
    [TestCase("fix(#123): description")]
    public void WhenScopeFormatIsInvalid_AndScopeRuleIsEnabled_ThenRuleIsNotValid(
        string commitMessage
    )
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = true };
        var rule = new ScopeRule(config);
        string[] commitMessageLines = new[] { commitMessage };

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Message.Should().NotBeEmpty();
    }

    [Test]
    public void WhenIsEmpty_AndRuleIsEnabled_ThenRuleIsNotValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = true };
        var rule = new ScopeRule(config);
        string[] commitMessageLines = new[] { "feat(): description" };

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Message.Should().NotBeEmpty();
    }

    [Test]
    public void WhenScopeContainsOnlyWhitespace_AndScopeRuleIsEnabled_ThenRuleIsNotValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = true };
        var rule = new ScopeRule(config);
        string[] commitMessageLines = new[] { "feat(   ): description" };

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Message.Should().NotBeEmpty();
    }

    [Test]
    public void WhenScopeIsFollowedByExclamationMark_AndRuleIsEnabled_ThenRuleIsValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = true };
        var rule = new ScopeRule(config);
        string[] commitMessageLines = new[] { "feat(scope)!: description" };

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
