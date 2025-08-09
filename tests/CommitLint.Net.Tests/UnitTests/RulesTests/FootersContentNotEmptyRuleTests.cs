using CommitLint.Net.Models;
using CommitLint.Net.Rules;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.RulesTests;

public class FootersContentNotEmptyRuleTests
{
    [Test]
    public void WhenCommitMessageIsEmpty_ThenRuleIsValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = true };
        var rule = new FootersContentNotEmptyRule(config);
        string[] commitMessageLines = [""];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void WhenFooterContentIsEmpty_ThenRuleIsNotValid()
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = true };
        var rule = new FootersContentNotEmptyRule(config);
        string[] commitMessageLines = ["fix: some bug fix", "", "Footer: "];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [TestCase("Footer:  ")]
    [TestCase("Footer:     ")]
    [TestCase("Footer: \t")]
    public void WhenFooterContentStartsWithWhitespace_ThenRuleIsNotValid(string footerContent)
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = true };
        var rule = new FootersContentNotEmptyRule(config);
        string[] commitMessageLines = ["fix: some bug fix", "", footerContent];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [TestCase("Footer: value")]
    [TestCase("some-footer #123")]
    public void WhenFooterContentIsNotEmpty_ThenRuleIsValid(string footerContent)
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = true };
        var rule = new FootersContentNotEmptyRule(config);
        string[] commitMessageLines = ["fix: some bug fix", "", footerContent];

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [TestCase("fix: description", "", "empty-footer: ", "valid-footer: content")]
    [TestCase("fix: description", "", "Footer: ", "Ref: valid reference", "Another-Footer:   ")]
    [TestCase("fix: description", "", "foot: valid", "invalid-footer: ", "Ref: reference")]
    [TestCase("fix: description", "Footer: ", "Footer: ")]
    public void WhenThereIsAtLeastOneInvalidFooter_ThenRuleIsNotValid(
        params string[] commitMessageLines
    )
    {
        // Arrange
        var config = new ConventionalCommitConfig { Enabled = true };
        var rule = new FootersContentNotEmptyRule(config);

        // Act
        var result = rule.IsValid(commitMessageLines);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
