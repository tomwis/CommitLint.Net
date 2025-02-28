using System.IO.Abstractions;
using System.Text.Json;
using CommitLint.Net.Models;

namespace CommitLint.Net;

public sealed class LinterConfig
{
    public LinterConfig(string commitMessageFileName, string commitMessageConfigFileName)
        : this(commitMessageFileName, commitMessageConfigFileName, new FileSystem()) { }

    public LinterConfig(
        string commitMessageFileName,
        string commitMessageConfigFileName,
        IFileSystem fileSystem
    )
    {
        fileSystem ??= new FileSystem();
        var commitMessageFile = fileSystem.FileInfo.New(commitMessageFileName);
        var commitMessageConfigFile = fileSystem.FileInfo.New(commitMessageConfigFileName);

        CommitMessageLines = fileSystem.File.ReadAllLines(commitMessageFile.FullName);
        var commitMessageConfigContent = fileSystem.File.ReadAllText(
            commitMessageConfigFile.FullName
        );
        var commitMessageConfigRoot = JsonSerializer.Deserialize<CommitMessageConfigRoot>(
            commitMessageConfigContent,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower }
        );
        CommitMessageConfig = commitMessageConfigRoot?.Config;
    }

    public string[] CommitMessageLines { get; }
    public CommitMessageConfig? CommitMessageConfig { get; }
}
