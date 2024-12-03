using CommitLint.Net.Models;
using CommitLint.Net.Rules.Models;

namespace CommitLint.Net.Rules;

public sealed class MaxSubjectLengthRule : Rule<MaxSubjectLength>
{
    public MaxSubjectLengthRule(MaxSubjectLength? maxSubjectLength)
        : base(maxSubjectLength) { }

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
