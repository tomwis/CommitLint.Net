namespace CommitLint.Net.Validators;

internal record ValidationResult
{
    private ValidationResult() { }

    public bool IsValid { get; init; }

    public static ValidationResult Valid() => new() { IsValid = true };

    public static ValidationResult Invalid() => new() { IsValid = false };
}
