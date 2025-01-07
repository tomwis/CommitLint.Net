using CommitLint.Net.Models;
using CommitLint.Net.Rules.Models;

namespace CommitLint.Net.Rules;

public sealed class BlankLineBeforeBodyRule(ConventionalCommitConfig? config)
    : Rule<ConventionalCommitConfig>(config)
{
    public override bool IsEnabled => Config?.Enabled ?? false;
    public override string Name => "Blank line must exist before body";

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
