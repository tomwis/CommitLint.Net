using CommitLint.Net.Rules.Models;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.RulesTests;

public class RuleTests
{
    [Test]
    public void WhenConfigurationIsNull_ThenRuleIsValid()
    {
        // Arrange
        string? config = null;
        var rule = new MockRule(config);

        // Act
        var result = rule.IsValid([]);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void WhenRuleIsDisabled_ThenRuleIsValid()
    {
        // Arrange
        const string config = "any config";
        var rule = new MockRule(config, isEnabled: false);

        // Act
        var result = rule.IsValid([]);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void WhenRuleIsEnabled_ThenIsValidInternalIsCalled()
    {
        // Arrange
        const string config = "any config";
        var rule = new MockRule(config, isEnabled: true);

        // Act
        _ = rule.IsValid([]);

        // Assert
        rule.IsValidInternalCalled.Should().BeTrue();
    }

    private class MockRule(string? config, bool isEnabled = false) : Rule<string>(config)
    {
        private bool _isValidInternalCalled;
        public override bool IsEnabled => isEnabled;

        protected override RuleValidationResult IsValidInternal(string[] commitMessageLines)
        {
            _isValidInternalCalled = true;
            return RuleValidationResult.Success();
        }

        public bool IsValidInternalCalled => _isValidInternalCalled;
    }
}
