using CommitLint.Net.Models;

namespace CommitLint.Net.Rules;

public sealed class MaxSubjectLengthRule(MaxSubjectLength? maxSubjectLength)
    : Rule<MaxSubjectLength>(maxSubjectLength)
{
    public override bool IsEnabled => Config?.Enabled ?? false;

    protected override RuleValidationResult IsValidInternal(string[] commitMessageLines)
    {
        if (commitMessageLines.Length < 1)
        {
            return RuleValidationResult.Failure("Commit message is empty.");
        }

        var commitSubjectLength = commitMessageLines[0].Length;
        if (commitSubjectLength > Config!.Value)
        {
            return RuleValidationResult.Failure(
                $"Commit subject is too long ({commitSubjectLength} characters). Should have max {Config.Value} characters."
            );
        }

        return RuleValidationResult.Success($"{commitSubjectLength}/{Config.Value} characters");
    }
}

public sealed class TypeValidationRule(ConventionalCommitConfig? config)
    : Rule<ConventionalCommitConfig>(config)
{
    private const string SubjectSeparator = ": ";
    public override bool IsEnabled => Config?.Enabled ?? false;

    protected override RuleValidationResult IsValidInternal(string[] commitMessageLines)
    {
        var allowedTypes = Config!.Types?.Select(p => p.ToLowerInvariant()).ToList();

        var commitType = commitMessageLines[0].Split(SubjectSeparator)[0].Split('(')[0];
        if (allowedTypes is not null && allowedTypes.All(t => t != commitType))
        {
            return RuleValidationResult.Failure(
                $"Commit type is not on accepted list. Current: {commitType}. Should be one of: {string.Join(", ", allowedTypes)}"
            );
        }

        return RuleValidationResult.Success();
    }
}

public sealed class DescriptionNotEmptyValidationRule(ConventionalCommitConfig? config)
    : Rule<ConventionalCommitConfig>(config)
{
    private const string SubjectSeparator = ": ";
    public override bool IsEnabled => Config?.Enabled ?? false;

    protected override RuleValidationResult IsValidInternal(string[] commitMessageLines)
    {
        var commitDescription = commitMessageLines[0].Split(SubjectSeparator)[1];
        if (string.IsNullOrWhiteSpace(commitDescription))
        {
            return RuleValidationResult.Failure("Commit description cannot be empty.");
        }

        return RuleValidationResult.Success();
    }
}

public sealed class BlankLineBetweenSubjectAndBodyValidationRule(ConventionalCommitConfig? config)
    : Rule<ConventionalCommitConfig>(config)
{
    public override bool IsEnabled => Config?.Enabled ?? false;

    protected override RuleValidationResult IsValidInternal(string[] commitMessageLines)
    {
        if (commitMessageLines.Length < 2)
        {
            return RuleValidationResult.Success("Commit message doesn't have body.");
        }

        if (!string.IsNullOrWhiteSpace(commitMessageLines[1]))
        {
            return RuleValidationResult.Failure(
                "There must be blank line between subject and body."
            );
        }

        return RuleValidationResult.Success();
    }
}

public sealed class BodyNotEmptyValidationRule(ConventionalCommitConfig? config)
    : Rule<ConventionalCommitConfig>(config)
{
    public override bool IsEnabled => Config?.Enabled ?? false;

    protected override RuleValidationResult IsValidInternal(string[] commitMessageLines)
    {
        if (commitMessageLines.Length < 3)
        {
            return RuleValidationResult.Success("No body to validate");
        }

        if (string.IsNullOrWhiteSpace(commitMessageLines[2]))
        {
            return RuleValidationResult.Failure("Body cannot be empty.");
        }

        return RuleValidationResult.Success();
    }
}
