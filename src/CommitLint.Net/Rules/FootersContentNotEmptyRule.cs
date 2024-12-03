using System.Text.RegularExpressions;
using CommitLint.Net.Models;
using CommitLint.Net.Rules.Models;

namespace CommitLint.Net.Rules;

public partial class FootersContentNotEmptyRule(ConventionalCommitConfig? config)
    : Rule<ConventionalCommitConfig>(config)
{
    private const string TokenGroupName = "token";

    public override bool IsEnabled => Config?.Enabled ?? false;

    private static readonly Regex FooterPattern = FooterTokenWithSeparatorRegex();
    private static readonly Regex BreakingChangePattern = BreakingChangeTokenWithSeparatorRegex();

    protected override RuleValidationResult IsValidInternal(string[] commitMessageLines)
    {
        var linesWithoutSubject = commitMessageLines.Skip(1).ToArray();
        var invalidFooterMessages = new List<string>();
        var validFooters = new List<List<string>>();

        foreach (var line in linesWithoutSubject)
        {
            var match = FindFooterMatch(line);

            if (!match.Success)
            {
                validFooters.LastOrDefault()?.Add(line);
                continue;
            }

            var validationError = ValidateFooterContent(
                line,
                match.Groups[TokenGroupName].Value,
                match.Length
            );

            if (!string.IsNullOrEmpty(validationError))
            {
                invalidFooterMessages.Add(validationError);
                continue;
            }

            validFooters.Add([line]);
        }

        return GenerateValidationResult(invalidFooterMessages, validFooters);
    }

    private static Match FindFooterMatch(string line)
    {
        return BreakingChangePattern.IsMatch(line)
            ? BreakingChangePattern.Match(line)
            : FooterPattern.Match(line);
    }

    private static string? ValidateFooterContent(
        string line,
        string token,
        int tokenWithSeparatorLength
    )
    {
        var content = line[tokenWithSeparatorLength..];
        if (string.IsNullOrWhiteSpace(content))
        {
            return $"Footer '{token}' is missing content.";
        }

        if (char.IsWhiteSpace(content[0]))
        {
            return $"Footer '{token}' starts with whitespace.";
        }

        return null;
    }

    private static RuleValidationResult GenerateValidationResult(
        List<string> invalidFooterMessages,
        List<List<string>> validFooters
    )
    {
        var newLine = Environment.NewLine;

        if (invalidFooterMessages.Count > 0)
        {
            var invalidFooterMessage = string.Join($"\t{newLine}", invalidFooterMessages);
            return RuleValidationResult.Failure(
                $"Invalid footers found:{newLine}{invalidFooterMessage}"
            );
        }

        if (validFooters.Count > 0)
        {
            var validFooterMessage = string.Join($"\t{newLine}", validFooters);
            return RuleValidationResult.Success(
                $"Valid footers found:{newLine}{validFooterMessage}"
            );
        }

        return RuleValidationResult.Success("No footers found.");
    }

    [GeneratedRegex($"^(?<{TokenGroupName}>[a-zA-Z0-9-]+)(: | #)", RegexOptions.Multiline)]
    private static partial Regex FooterTokenWithSeparatorRegex();

    [GeneratedRegex("^(BREAKING[ -]CHANGE): ", RegexOptions.Multiline)]
    private static partial Regex BreakingChangeTokenWithSeparatorRegex();
}
