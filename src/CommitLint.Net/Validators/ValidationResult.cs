namespace CommitLint.Net.Validators;

internal record ValidationResult
{
    private ValidationResult() { }

    public bool IsValid { get; private init; }
    public string? Message { get; private init; }

    public static ValidationResult Valid() => new() { IsValid = true };

    public static ValidationResult Invalid(string? resultMessage) =>
        new() { IsValid = false, Message = resultMessage };
}
