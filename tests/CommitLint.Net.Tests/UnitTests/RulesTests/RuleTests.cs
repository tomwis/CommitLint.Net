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
        var result = rule.IsValid(Array.Empty<string>());

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
        var result = rule.IsValid(Array.Empty<string>());

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
        _ = rule.IsValid(Array.Empty<string>());

        // Assert
        rule.IsValidInternalCalled.Should().BeTrue();
    }

    private class MockRule : Rule<string>
    {
        private bool _isValidInternalCalled;
        private readonly bool _isEnabled;

        public MockRule(string? config, bool isEnabled = false)
            : base(config)
        {
            _isEnabled = isEnabled;
        }

        public override bool IsEnabled => _isEnabled;

        protected override RuleValidationResult IsValidInternal(string[] commitMessageLines)
        {
            _isValidInternalCalled = true;
            return RuleValidationResult.Success();
        }

        public bool IsValidInternalCalled => _isValidInternalCalled;
    }
}
