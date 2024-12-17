using CommitLint.Net.Logging;
using CommitLint.Net.Validators;

namespace CommitLint.Net;

public sealed class Linter
{
    public void Run(LinterConfig config)
    {
        PrintCommit(config.CommitMessageLines);

        var commitMessageValidator = new CommitMessageValidator(config.CommitMessageConfig);
        var result = commitMessageValidator.Validate(config.CommitMessageLines);

        if (!result.IsValid)
        {
            throw new CommitFormatException(
                $"Commit message is in invalid format. Error: {result.Message}"
            );
        }
    }

    private static void PrintCommit(string[] commitMessage)
    {
        Log.Verbose("Commit message with line numbers:");
        for (int i = 0; i < commitMessage.Length; ++i)
        {
            Log.Verbose($"    {i}: {commitMessage[i]}");
        }

        Log.Verbose(Environment.NewLine);
    }
}
