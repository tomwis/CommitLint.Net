using System.Text.RegularExpressions;
using CommitLint.Net.Models;
using CommitLint.Net.Rules.Models;

namespace CommitLint.Net.Rules;

public sealed class ScopeRule(ConventionalCommitConfig? config)
    : Rule<ConventionalCommitConfig>(config)
{
    private const string SubjectSeparator = ": ";
    public override bool IsEnabled => Config?.Enabled ?? false;

    private RuleValidationResult FormatFailure() =>
        RuleValidationResult.Failure(
            "Scope is not in the correct format. Should be 'type(scope): description'"
        );

    protected override RuleValidationResult IsValidInternal(string[] commitMessageLines)
    {
        var typeAndScope = commitMessageLines[0].Split(SubjectSeparator)[0];
        var openParenthesisIndex = typeAndScope.IndexOf('(');
        var closeParenthesisIndex = typeAndScope.IndexOf(')');

        if (openParenthesisIndex == -1 && closeParenthesisIndex == -1)
        {
            return RuleValidationResult.Success("No scope found.");
        }

        if (!typeAndScope.EndsWith(')'))
            return FormatFailure();

        var commitScope = typeAndScope.Substring(
            openParenthesisIndex + 1,
            closeParenthesisIndex - openParenthesisIndex - 1
        );

        if (string.IsNullOrWhiteSpace(commitScope))
            return RuleValidationResult.Failure("Scope shouldn't be empty.");

        if (!Regex.IsMatch(commitScope, @"^\w+$"))
            return RuleValidationResult.Failure("Scope should be 1 word (noun).");

        return RuleValidationResult.Success();
    }
}
