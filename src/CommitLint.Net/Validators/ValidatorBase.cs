using CommitLint.Net.Rules.Models;

namespace CommitLint.Net.Validators;

internal abstract class ValidatorBase
{
    protected abstract List<IRule> Rules { get; }

    public ValidationResult Validate(string[] commitMessageLines)
    {
        foreach (var rule in Rules)
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
