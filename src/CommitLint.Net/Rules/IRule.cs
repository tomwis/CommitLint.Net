namespace CommitLint.Net.Rules;

public interface IRule
{
    string Name { get; }
    bool IsEnabled { get; }
    RuleValidationResult IsValid(string[] commitMessageLines);
}
