using CommitLint.Net.Models;
using CommitLint.Net.Rules;
using CommitLint.Net.Rules.Models;

namespace CommitLint.Net.Validators;

internal class ConventionalCommitsSpecValidator(CommitMessageConfig? config) : ValidatorBase
{
    protected override List<IRule> Rules { get; } =
        [
            new BreakingChangeSubjectMarkerRule(config?.ConventionalCommit),
            new TypeRule(config?.ConventionalCommit),
            new ScopeRule(config?.ConventionalCommit),
            new DescriptionNotEmptyRule(config?.ConventionalCommit),
            new BlankLineBeforeBodyRule(config?.ConventionalCommit),
            new BodyNotEmptyRule(config?.ConventionalCommit),
            new BreakingChangeTokenInFooterRule(config?.ConventionalCommit),
            new BlankLineBeforeFootersRule(config?.ConventionalCommit),
            new FootersContentNotEmptyRule(config?.ConventionalCommit),
        ];
}
