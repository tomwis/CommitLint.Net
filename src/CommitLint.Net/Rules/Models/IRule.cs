namespace CommitLint.Net.Rules.Models;

public interface IRule
{
    string Name { get; }
    bool IsEnabled { get; }
    RuleValidationResult IsValid(string[] commitMessageLines);
}
