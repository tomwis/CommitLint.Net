using CommitLint.Net.Models;
using CommitLint.Net.Rules.Models;

namespace CommitLint.Net.Rules;

public sealed class DescriptionNotEmptyRule(ConventionalCommitConfig? config)
    : Rule<ConventionalCommitConfig>(config)
{
    public override bool IsEnabled => Config?.Enabled ?? false;
    public override string Name => "Description must not be empty";

    protected override RuleValidationResult IsValidInternal(string[] commitMessageLines)
    {
        var commitDescription = commitMessageLines[0].Split(RuleConstants.SubjectSeparator)[1];
        if (string.IsNullOrWhiteSpace(commitDescription))
        {
            return RuleValidationResult.Failure("Commit description cannot be empty.");
        }

        return RuleValidationResult.Success();
    }
}
