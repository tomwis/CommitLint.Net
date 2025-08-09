using CommitLint.Net.Models;
using CommitLint.Net.Rules.Models;

namespace CommitLint.Net.Rules;

public sealed class BreakingChangeSubjectMarkerRule(ConventionalCommitConfig? config)
    : Rule<ConventionalCommitConfig>(config)
{
    public override bool IsEnabled => Config?.Enabled ?? false;
    public override string Name =>
        "Breaking change marker (!) in subject must be in correct format";

    protected override RuleValidationResult IsValidInternal(string[] commitMessageLines)
    {
        if (commitMessageLines.Length < 1)
        {
            return RuleValidationResult.Success("Commit message is empty.");
        }

        var typeAndScope = commitMessageLines[0].Split(RuleConstants.SubjectSeparator)[0];

        var exclamationCount = typeAndScope.Count(c => c == '!');
        return exclamationCount switch
        {
            > 1 => FormatFailure(),
            1 when !typeAndScope.EndsWith('!') => FormatFailure(),
            _ => RuleValidationResult.Success(),
        };
    }

    private static RuleValidationResult FormatFailure() =>
        RuleValidationResult.Failure(
            "Breaking change format is not correct. Should be 'type(scope)!: description' or 'type!: description'"
        );
}
