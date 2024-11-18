using System.IO.Abstractions;

namespace CommitLint.Net.Tests.UnitTests.Extensions;

public static class StringExtensions
{
    public static string ToCommitFile(this string content, IFileSystem fileSystem)
    {
        const string filePath = "commit.txt";
        fileSystem.File.WriteAllText(filePath, content);
        return filePath;
    }
}
