using CommitLint.Net.Models;
using CommitLint.Net.Rules.Models;

namespace CommitLint.Net.Rules;

public sealed class TypeRule(ConventionalCommitConfig? config)
    : Rule<ConventionalCommitConfig>(config)
{
    public override bool IsEnabled => Config?.Enabled ?? false;

    protected override RuleValidationResult IsValidInternal(string[] commitMessageLines)
    {
        var allowedTypes = Config!.Types?.Select(p => p.ToLowerInvariant()).ToList();

        var commitType = commitMessageLines[0].Split(RuleConstants.SubjectSeparator)[0].Split('(')[
            0
        ];
        if (commitType.EndsWith('!'))
        {
            commitType = commitType[..^1];
        }

        if (allowedTypes is not null && allowedTypes.All(t => t != commitType))
        {
            return RuleValidationResult.Failure(
                $"Commit type is not on accepted list. Current: {commitType}. Should be one of: {string.Join(", ", allowedTypes)}"
            );
        }

        return RuleValidationResult.Success();
    }
}
