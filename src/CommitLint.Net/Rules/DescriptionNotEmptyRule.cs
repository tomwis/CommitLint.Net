using CommitLint.Net.Models;
using CommitLint.Net.Rules.Models;

namespace CommitLint.Net.Rules;

public sealed class DescriptionNotEmptyRule : Rule<ConventionalCommitConfig>
{
    public DescriptionNotEmptyRule(ConventionalCommitConfig? config)
        : base(config) { }

    public override bool IsEnabled => Config?.Enabled ?? false;

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
