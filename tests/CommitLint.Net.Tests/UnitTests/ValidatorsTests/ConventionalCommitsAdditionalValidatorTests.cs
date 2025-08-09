using CommitLint.Net.Models;
using CommitLint.Net.Validators;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.ValidatorsTests;

[TestFixture]
public class ConventionalCommitsAdditionalValidatorTests
{
    [TestCaseSource(nameof(ValidCommitMessages))]
    public void WhenCommitMessageIsValid_ThenReturnValidResult(string[] commitMessageLines)
    {
        // Arrange
        var subject = new ConventionalCommitsAdditionalValidator(GetConfig());

        // Act
        var result = subject.Validate(commitMessageLines);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [TestCaseSource(nameof(ValidCommitMessages))]
    public void WhenCommitMessageIsValid_AndScopeIsRestrictedGlobally_ThenReturnValidResult(
        string[] commitMessageLines
    )
    {
        // Arrange
        var subject = new ConventionalCommitsAdditionalValidator(GetConfigWithGlobalScopes());

        // Act
        var result = subject.Validate(commitMessageLines);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [TestCaseSource(nameof(ValidCommitMessages))]
    public void WhenCommitMessageIsValid_AndScopeIsRestrictedPerType_ThenReturnValidResult(
        string[] commitMessageLines
    )
    {
        // Arrange
        var subject = new ConventionalCommitsAdditionalValidator(GetConfigWithPerTypeScopes());

        // Act
        var result = subject.Validate(commitMessageLines);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [TestCaseSource(nameof(InvalidCommitMessagesForScopeRestriction))]
    public void WhenCommitMessageHasNotAllowedScope_AndScopeIsRestrictedGlobally_ThenReturnInvalidResult(
        string[] commitMessageLines
    )
    {
        // Arrange
        var subject = new ConventionalCommitsAdditionalValidator(GetConfigWithGlobalScopes());

        // Act
        var result = subject.Validate(commitMessageLines);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [TestCaseSource(nameof(InvalidCommitMessagesForScopeRestriction))]
    public void WhenCommitMessageHasNotAllowedScope_AndScopeIsRestrictedPerType_ThenReturnInvalidResult(
        string[] commitMessageLines
    )
    {
        // Arrange
        var subject = new ConventionalCommitsAdditionalValidator(GetConfigWithPerTypeScopes());

        // Act
        var result = subject.Validate(commitMessageLines);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    private static CommitMessageConfig GetConfig()
    {
        return new CommitMessageConfig
        {
            ConventionalCommit = new ConventionalCommitConfig
            {
                Enabled = true,
                Types = ["feat", "fix", "docs", "revert"],
            },
        };
    }

    private static CommitMessageConfig GetConfigWithGlobalScopes()
    {
        var config = GetConfig();
        config.ConventionalCommit!.Scopes = new ScopesConfig { Enabled = true, Global = ["scope"] };
        return config;
    }

    private static CommitMessageConfig GetConfigWithPerTypeScopes()
    {
        var config = GetConfig();
        config.ConventionalCommit!.Scopes = new ScopesConfig
        {
            Enabled = true,
            PerType = new Dictionary<string, List<string>>
            {
                ["feat"] = ["scope"],
                ["fix"] = ["scope2"],
            },
        };
        return config;
    }

    private static IEnumerable<string[]> InvalidCommitMessagesForScopeRestriction()
    {
        yield return ["feat(invalid-scope): commit message"];
        yield return ["fix(wrong): commit message"];
    }

    private static IEnumerable<string[]> ValidCommitMessages()
    {
        yield return ["feat: valid commit message"];
        yield return ["fix: valid fix commit message"];
        yield return ["docs: readme update", "", "body of valid commit message"];
        yield return ["feat(scope): some update"];
        yield return ["feat(scope)!: some update"];
        yield return ["feat!: some update"];
        yield return
        [
            "docs: readme update",
            "",
            "body of valid commit message",
            "",
            "BREAKING CHANGE: some breaking change",
        ];
    }
}
