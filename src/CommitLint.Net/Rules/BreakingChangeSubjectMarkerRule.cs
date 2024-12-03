using CommitLint.Net.Models;
using CommitLint.Net.Rules.Models;

namespace CommitLint.Net.Rules;

public sealed class BreakingChangeSubjectMarkerRule : Rule<ConventionalCommitConfig>
{
    public BreakingChangeSubjectMarkerRule(ConventionalCommitConfig? config)
        : base(config) { }

    public override bool IsEnabled => Config?.Enabled ?? false;

    protected override RuleValidationResult IsValidInternal(string[] commitMessageLines)
    {
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
