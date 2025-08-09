using CommitLint.Net.Models;
using CommitLint.Net.Rules.Models;

namespace CommitLint.Net.Rules;

public sealed class AllowedScopesRule(ConventionalCommitConfig? config)
    : Rule<ConventionalCommitConfig>(config)
{
    public override bool IsEnabled => Config?.Scopes?.Enabled == true;
    public override string Name => "Scope must match configured values";

    protected override RuleValidationResult IsValidInternal(string[] commitMessageLines)
    {
        var typeAndScope = commitMessageLines[0].Split(RuleConstants.SubjectSeparator)[0];
        var openParenthesisIndex = typeAndScope.IndexOf('(');
        var closeParenthesisIndex = typeAndScope.IndexOf(')');

        if (openParenthesisIndex == -1 && closeParenthesisIndex == -1)
        {
            return RuleValidationResult.Success("No scope found.");
        }

        var commitScope = typeAndScope.Substring(
            openParenthesisIndex + 1,
            closeParenthesisIndex - openParenthesisIndex - 1
        );

        var commitType = typeAndScope[..openParenthesisIndex].ToLowerInvariant();

        if (Config?.Scopes?.Global != null)
        {
            return ValidateScopeValue(Config.Scopes.Global, commitScope);
        }

        if (
            Config?.Scopes?.PerType != null
            && Config.Scopes.PerType.TryGetValue(commitType, out var perTypeScopes)
        )
        {
            return ValidateScopeValue(perTypeScopes, commitScope);
        }

        return RuleValidationResult.Success("No scope restrictions configured.");
    }

    private static RuleValidationResult ValidateScopeValue(
        List<string> allowedScopes,
        string commitScope
    )
    {
        if (allowedScopes.Count == 0)
        {
            return RuleValidationResult.Success("No scope restrictions configured.");
        }

        if (
            !allowedScopes.Any(s =>
                string.Equals(s, commitScope, StringComparison.OrdinalIgnoreCase)
            )
        )
        {
            var allowedScopesList = string.Join(", ", allowedScopes);
            return RuleValidationResult.Failure(
                $"Scope '{commitScope}' is not allowed. Allowed scopes: {allowedScopesList}"
            );
        }

        return RuleValidationResult.Success("No scope restrictions configured.");
    }
}
