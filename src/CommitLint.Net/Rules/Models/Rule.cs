using CommitLint.Net.Logging;

namespace CommitLint.Net.Rules.Models;

public abstract class Rule<T>(T? config) : IRule
{
    private const char Checkmark = '\u2714';
    private const char Cross = '\u2718';

    protected T? Config { get; } = config;
    public abstract bool IsEnabled { get; }
    public string Name => GetType().Name;

    public RuleValidationResult IsValid(string[] commitMessageLines)
    {
        LogStart();
        if (Config is null)
        {
            Log.Info("\tConfiguration value not found. Not validating.");
            return RuleValidationResult.ConfigNull();
        }

        if (!IsEnabled)
        {
            Log.Info("\tCheck disabled");
            return RuleValidationResult.Disabled();
        }

        Log.Verbose("\tCheck enabled");
        var result = IsValidInternal(commitMessageLines);
        var message = result.IsValid ? $"Check passed {Checkmark}" : $"Check failed {Cross}";
        if (!string.IsNullOrEmpty(result.Message))
        {
            message += $" - {result.Message}";
        }
        Log.Info($"\t{message}");
        return result;
    }

    protected abstract RuleValidationResult IsValidInternal(string[] commitMessageLines);

    private void LogStart()
    {
        Log.Info($"Rule {Name}");
    }
}
