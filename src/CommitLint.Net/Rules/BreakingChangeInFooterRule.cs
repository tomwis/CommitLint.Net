using CommitLint.Net.Models;
using CommitLint.Net.Rules.Models;

namespace CommitLint.Net.Rules;

public class BreakingChangeInFooterRule(ConventionalCommitConfig? config)
    : Rule<ConventionalCommitConfig>(config)
{
    public override bool IsEnabled => Config?.Enabled ?? false;

    protected override RuleValidationResult IsValidInternal(string[] commitMessageLines)
    {
        const string breakingChangeToken = "BREAKING CHANGE: ";
        var linesWithBreakingChangeTokenCaseInsensitive = commitMessageLines
            .Where(line => line.StartsWith(breakingChangeToken, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var tokens = linesWithBreakingChangeTokenCaseInsensitive.Select(line =>
            line.Substring(0, breakingChangeToken.Length)
        );

        if (tokens.Any(token => token != breakingChangeToken))
        {
            return RuleValidationResult.Failure("BREAKING CHANGE token must be in upper case.");
        }

        var linesWithBreakingChangeToken = commitMessageLines
            .Where(line => line.StartsWith(breakingChangeToken))
            .ToList();

        if (linesWithBreakingChangeToken.Count == 0)
        {
            return RuleValidationResult.Success("No BREAKING CHANGE footer found.");
        }

        if (linesWithBreakingChangeToken.Count > 1)
        {
            return RuleValidationResult.Failure("Only one BREAKING CHANGE footer is allowed.");
        }

        var breakingChangeTokenIndex = commitMessageLines
            .ToList()
            .IndexOf(linesWithBreakingChangeToken[0]);
        if (breakingChangeTokenIndex == 0)
        {
            return RuleValidationResult.Failure("BREAKING CHANGE token cannot be in subject.");
        }

        if (!string.IsNullOrEmpty(commitMessageLines[breakingChangeTokenIndex - 1]))
        {
            return RuleValidationResult.Failure(
                "BREAKING CHANGE footer must have blank line before."
            );
        }

        var breakingChangeFooter = commitMessageLines[breakingChangeTokenIndex];
        var breakingChangeDescription = breakingChangeFooter[breakingChangeToken.Length..];

        if (string.IsNullOrWhiteSpace(breakingChangeDescription))
        {
            return RuleValidationResult.Failure(
                "BREAKING CHANGE footer content shouldn't be empty."
            );
        }

        return RuleValidationResult.Success();
    }
}
