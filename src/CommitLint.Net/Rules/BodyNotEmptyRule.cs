using CommitLint.Net.Models;
using CommitLint.Net.Rules.Models;

namespace CommitLint.Net.Rules;

public sealed class BodyNotEmptyRule(ConventionalCommitConfig? config)
    : Rule<ConventionalCommitConfig>(config)
{
    public override bool IsEnabled => Config?.Enabled ?? false;
    public override string Name => "Body must not be empty";

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
