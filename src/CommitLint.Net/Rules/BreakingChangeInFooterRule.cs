using CommitLint.Net.Models;
using CommitLint.Net.Rules.Models;

namespace CommitLint.Net.Rules;

public class BreakingChangeInFooterRule(ConventionalCommitConfig? config)
    : Rule<ConventionalCommitConfig>(config)
{
    private const string BreakingChangeToken = "BREAKING CHANGE: ";
    private const string BreakingChangeHyphenToken = "BREAKING-CHANGE: ";

    public override bool IsEnabled => Config?.Enabled ?? false;

    protected override RuleValidationResult IsValidInternal(string[] commitMessageLines)
    {
        if (!IsTokenUppercase(commitMessageLines))
        {
            return RuleValidationResult.Failure("BREAKING CHANGE token must be in upper case.");
        }

        var linesWithBreakingChangeToken = commitMessageLines
            .Where(line =>
                line.StartsWith(BreakingChangeToken) || line.StartsWith(BreakingChangeHyphenToken)
            )
            .ToList();

        if (linesWithBreakingChangeToken.Count == 0)
        {
            return RuleValidationResult.Success("No BREAKING CHANGE footer found.");
        }

        foreach (var breakingChangeFooter in linesWithBreakingChangeToken)
        {
            var breakingChangeDescription = breakingChangeFooter[BreakingChangeToken.Length..];
            if (string.IsNullOrWhiteSpace(breakingChangeDescription))
            {
                return RuleValidationResult.Failure(
                    "BREAKING CHANGE footer content shouldn't be empty."
                );
            }
        }

        return RuleValidationResult.Success();
    }

    private static bool IsTokenUppercase(string[] commitMessageLines)
    {
        var linesWithBreakingChangeTokenCaseInsensitive = commitMessageLines
            .Where(line =>
                line.StartsWith(BreakingChangeToken, StringComparison.OrdinalIgnoreCase)
                || line.StartsWith(BreakingChangeHyphenToken, StringComparison.OrdinalIgnoreCase)
            )
            .ToList();

        var tokens = linesWithBreakingChangeTokenCaseInsensitive.Select(line =>
            line[..BreakingChangeToken.Length]
        );

        return tokens.All(token => token is BreakingChangeToken or BreakingChangeHyphenToken);
    }
}
