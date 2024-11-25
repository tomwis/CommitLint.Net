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
        Console.WriteLine("Commit message with line numbers:");
        for (int i = 0; i < commitMessage.Length; ++i)
        {
            Console.WriteLine($"    {i}: {commitMessage[i]}");
        }

        Console.WriteLine();
    }
}
