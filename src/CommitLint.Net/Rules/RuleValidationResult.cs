namespace CommitLint.Net.Rules;

public class RuleValidationResult
{
    private RuleValidationResult() { }

    public bool IsValid { get; private init; }
    public string? Message { get; private init; }

    public static RuleValidationResult ConfigNull() =>
        new() { IsValid = true, Message = "Configuration value not found. Not validating." };

    public static RuleValidationResult Disabled() =>
        new() { IsValid = true, Message = "Check disabled." };

    public static RuleValidationResult Failure(string errorMessage) =>
        new() { IsValid = false, Message = errorMessage };

    public static RuleValidationResult Success(string? message = null) =>
        new() { IsValid = true, Message = message };
}
