namespace CommitLint.Net.Rules;

public abstract class Rule<T>(T? config) : IRule
{
    private const char Checkmark = '\u2714';
    private const char Cross = '\u2718';

    protected T? Config { get; } = config;
    public abstract bool IsEnabled { get; }
    public string Name => GetType().Name;

    public virtual RuleValidationResult IsValid(string[] commitMessageLines)
    {
        LogStart();
        if (Config is null)
        {
            Log("Configuration value not found. Not validating.");
            return RuleValidationResult.ConfigNull();
        }

        if (!IsEnabled)
        {
            Log("Check disabled");
            return RuleValidationResult.Disabled();
        }

        Log("Check enabled");
        var result = IsValidInternal(commitMessageLines);
        var message = result.IsValid ? $"Check passed {Checkmark}" : $"Check failed {Cross}";
        if (!string.IsNullOrEmpty(result.Message))
        {
            message += $" - {result.Message}";
        }
        Log(message);
        return result;
    }

    protected abstract RuleValidationResult IsValidInternal(string[] commitMessageLines);

    private void Log(string message)
    {
        Console.WriteLine($"\t{message}");
    }

    private void LogStart()
    {
        Console.WriteLine($"Rule {Name}");
    }
}
