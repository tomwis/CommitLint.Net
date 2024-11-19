namespace CommitLint.Net.Validators;

internal interface IValidator
{
    ValidationResult Validate(string[] commitMessageLines);
}
