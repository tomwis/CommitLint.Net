using CommitLint.Net.Models;
using CommitLint.Net.Rules.Models;

namespace CommitLint.Net.Rules;

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
