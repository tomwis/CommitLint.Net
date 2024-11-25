using CommitLint.Net.Models;
using CommitLint.Net.Rules;
using CommitLint.Net.Rules.Models;

namespace CommitLint.Net.Validators;

internal class CommitMessageValidator(CommitMessageConfig? config)
{
    private readonly List<IRule> _rules =
    [
        new MaxSubjectLengthRule(config?.MaxSubjectLength),
        new BreakingChangeSubjectMarkerRule(config?.ConventionalCommit),
        new TypeRule(config?.ConventionalCommit),
        new ScopeRule(config?.ConventionalCommit),
        new DescriptionNotEmptyRule(config?.ConventionalCommit),
        new BlankLineBeforeBodyRule(config?.ConventionalCommit),
        new BodyNotEmptyRule(config?.ConventionalCommit),
        new BreakingChangeInFooterRule(config?.ConventionalCommit),
        new BlankLineBeforeFootersRule(config?.ConventionalCommit),
    ];

    public ValidationResult Validate(string[] commitMessageLines)
    {
        foreach (var rule in _rules)
        {
            var result = rule.IsValid(commitMessageLines);
            if (!result.IsValid)
            {
                return ValidationResult.Invalid(result.Message);
            }
        }

        return ValidationResult.Valid();
    }
}
