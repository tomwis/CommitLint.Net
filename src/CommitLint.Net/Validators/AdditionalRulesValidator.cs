using CommitLint.Net.Models;
using CommitLint.Net.Rules;
using CommitLint.Net.Rules.Models;

namespace CommitLint.Net.Validators;

internal class AdditionalRulesValidator(CommitMessageConfig? config) : ValidatorBase()
{
    protected override List<IRule> Rules { get; } =
        [new MaxSubjectLengthRule(config?.MaxSubjectLength)];
}
