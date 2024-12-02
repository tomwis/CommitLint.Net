using CommandLine;

namespace CommitLint.Net;

public class Options
{
    [Option(
        'c',
        "commit-file",
        Required = true,
        HelpText = "Provide file with commit message to validate."
    )]
    public string CommitMessageFileName { get; set; } = null!;

    [Option(
        'o',
        "commit-message-config-file",
        Required = false,
        HelpText = "Provide file name of commit message config in json format."
    )]
    public string? CommitMessageConfigFileName { get; set; } = null!;
}
