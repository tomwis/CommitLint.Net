using CommitLint.Net.Models;
using CommitLint.Net.Rules;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.RulesTests;

public class AllowedScopesRuleTests
{
    [Test]
    public void WhenScopesAreConfiguredAsGlobal_AndMessageHasNoScope_ThenRuleIsValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig
        {
            Enabled = true,
            Scopes = new ScopesConfig { Enabled = true, Global = ["api", "ui"] },
        };
        var rule = new AllowedScopesRule(config);
        string[] commitMessageLines = ["feat: description"];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void WhenScopesAreConfiguredPerType_AndMessageHasNoScope_ThenRuleIsValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig
        {
            Enabled = true,
            Scopes = new ScopesConfig
            {
                Enabled = true,
                PerType = new Dictionary<string, List<string>> { ["feat"] = ["api", "ui"] },
            },
        };
        var rule = new AllowedScopesRule(config);
        string[] commitMessageLines = ["feat: description"];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void WhenScopesAreNotConfigured_AndRuleIsEnabled_ThenRuleIsValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig
        {
            Enabled = true,
            Scopes = new ScopesConfig { Enabled = true },
        };
        var rule = new AllowedScopesRule(config);
        string[] commitMessageLines = ["feat(any-scope): description"];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void WhenScopesAreConfiguredAsEmpty_AndRuleIsEnabled_ThenRuleIsValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig
        {
            Enabled = true,
            Scopes = new ScopesConfig
            {
                Enabled = true,
                Global = [],
                PerType = new Dictionary<string, List<string>>(),
            },
        };
        var rule = new AllowedScopesRule(config);
        string[] commitMessageLines = ["feat(any-scope): description"];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void WhenScopesAreConfigured_AndScopeIsInGlobalList_ThenRuleIsValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig
        {
            Enabled = true,
            Scopes = new ScopesConfig { Enabled = true, Global = ["api", "ui"] },
        };
        var rule = new AllowedScopesRule(config);
        string[] commitMessageLines = ["feat(api): description"];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void WhenScopesAreConfiguredForGlobal_AndScopeIsNotInGlobalList_ThenRuleIsValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig
        {
            Enabled = true,
            Scopes = new ScopesConfig { Enabled = true, Global = ["api", "ui"] },
        };
        var rule = new AllowedScopesRule(config);
        string[] commitMessageLines = ["feat(frontend): description"];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Message.Should().Contain("is not allowed");
    }

    [Test]
    public void WhenScopesAreConfiguredPerType_AndScopeIsInTypeSpecificList_ThenRuleIsValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig
        {
            Enabled = true,
            Scopes = new ScopesConfig
            {
                Enabled = true,
                PerType = new Dictionary<string, List<string>>
                {
                    ["feat"] = ["api", "ui"],
                    ["fix"] = ["bug", "crash"],
                },
            },
        };
        var rule = new AllowedScopesRule(config);
        string[] commitMessageLines = ["feat(ui): description"];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void WhenScopesAreConfiguredForGlobal_AndScopeIsNotInGlobalList_ThenRuleIsNotValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig
        {
            Enabled = true,
            Scopes = new ScopesConfig { Enabled = true, Global = ["api", "ui"] },
        };
        var rule = new AllowedScopesRule(config);
        string[] commitMessageLines = ["feat(invalid): description"];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Message.Should().Contain("is not allowed");
    }

    [Test]
    public void WhenScopesAreConfiguredPerType_AndScopeIsNotInTypeSpecificList_ThenRuleIsNotValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig
        {
            Enabled = true,
            Scopes = new ScopesConfig
            {
                Enabled = true,
                PerType = new Dictionary<string, List<string>> { ["feat"] = ["ui"] },
            },
        };
        var rule = new AllowedScopesRule(config);
        string[] commitMessageLines = ["feat(invalid): description"];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Message.Should().Contain("is not allowed");
    }

    [Test]
    public void WhenScopesAreConfiguredPerTypeAndAndGlobal_AndTypeIsNotInPerTypeList_AndScopeIsNotInGlobalList_ThenRuleIsNotValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig
        {
            Enabled = true,
            Scopes = new ScopesConfig
            {
                Enabled = true,
                Global = ["api", "ui"],
                PerType = new Dictionary<string, List<string>> { ["fix"] = ["regression"] },
            },
        };
        var rule = new AllowedScopesRule(config);
        string[] commitMessageLines = ["feat(regression): description"];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Message.Should().Contain("is not allowed");
    }

    [Test]
    public void WhenScopesAreConfigured_AndRuleIsDisabled_ThenRuleIsValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig
        {
            Enabled = true,
            Scopes = new ScopesConfig { Enabled = false, Global = ["api", "ui"] },
        };
        var rule = new AllowedScopesRule(config);
        string[] commitMessageLines = ["feat(any-scope): description"];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
