using CommitLint.Net.Models;
using CommitLint.Net.Rules;
using CommitLint.Net.Rules.Models;

namespace CommitLint.Net.Validators;

internal class ConventionalCommitsAdditionalValidator(CommitMessageConfig? config) : ValidatorBase
{
    protected override List<IRule> Rules { get; } =
        [new AllowedScopesRule(config?.ConventionalCommit)];
}
