using System.Text.RegularExpressions;
using CommitLint.Net.Models;
using CommitLint.Net.Rules.Models;

namespace CommitLint.Net.Rules;

public partial class BlankLineBeforeFootersRule : Rule<ConventionalCommitConfig>
{
    public BlankLineBeforeFootersRule(ConventionalCommitConfig? config)
        : base(config) { }

    public override bool IsEnabled => Config?.Enabled ?? false;

    private static readonly Regex FooterPattern = FooterTokenWithSeparatorRegex();
    private static readonly Regex BreakingChangePattern = BreakingChangeTokenWithSeparatorRegex();

    protected override RuleValidationResult IsValidInternal(string[] commitMessageLines)
    {
        var linesWithoutSubject = commitMessageLines.Skip(1).ToArray();

        for (var i = 0; i < linesWithoutSubject.Length; i++)
        {
            var line = linesWithoutSubject[i];
            var match = FindFooterMatch(line);

            if (!match.Success)
                continue;

            if (i > 0 && string.IsNullOrWhiteSpace(linesWithoutSubject[i - 1]))
            {
                return RuleValidationResult.Success($"Blank line found before {match.Value}");
            }

            return RuleValidationResult.Failure("There must be blank line before footers.");
        }

        return RuleValidationResult.Success("No footers found.");
    }

    private static Match FindFooterMatch(string line)
    {
        return BreakingChangePattern.IsMatch(line)
            ? BreakingChangePattern.Match(line)
            : FooterPattern.Match(line);
    }

    [GeneratedRegex(@"^(?<token>[a-zA-Z0-9-]+)(: | #)", RegexOptions.Multiline)]
    private static partial Regex FooterTokenWithSeparatorRegex();

    [GeneratedRegex(@"^(BREAKING[ -]CHANGE): ", RegexOptions.Multiline)]
    private static partial Regex BreakingChangeTokenWithSeparatorRegex();
}
