using CommitLint.Net.Models;
using CommitLint.Net.Rules.Models;

namespace CommitLint.Net.Rules;

public class BreakingChangeTokenInFooterRule : Rule<ConventionalCommitConfig>
{
    public BreakingChangeTokenInFooterRule(ConventionalCommitConfig? config)
        : base(config) { }

    private const string BreakingChangeToken = "BREAKING CHANGE: ";
    private const string BreakingChangeHyphenToken = "BREAKING-CHANGE: ";

    public override bool IsEnabled => Config?.Enabled ?? false;

    protected override RuleValidationResult IsValidInternal(string[] commitMessageLines)
    {
        var linesWithBreakingChangeTokenCaseInsensitive = commitMessageLines
            .Where(line =>
                line.StartsWith(BreakingChangeToken, StringComparison.OrdinalIgnoreCase)
                || line.StartsWith(BreakingChangeHyphenToken, StringComparison.OrdinalIgnoreCase)
            )
            .ToList();

        if (linesWithBreakingChangeTokenCaseInsensitive.Count == 0)
        {
            return RuleValidationResult.Success("No BREAKING CHANGE footer found.");
        }

        var breakingChangeTokens = linesWithBreakingChangeTokenCaseInsensitive.Select(line =>
            line[..BreakingChangeToken.Length]
        );

        var areAllTokensUppercase = breakingChangeTokens.All(token =>
            token is BreakingChangeToken or BreakingChangeHyphenToken
        );

        if (!areAllTokensUppercase)
        {
            return RuleValidationResult.Failure("BREAKING CHANGE token must be in upper case.");
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
