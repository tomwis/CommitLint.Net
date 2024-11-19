using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Text.Json;
using CommitLint.Net.Models;
using CommitLint.Net.Tests.UnitTests.Extensions;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.LinterRulesTests;

public class LinterRulesTestsBase
{
    protected IFileSystem MockFileSystem { get; private set; }

    protected CommitMessage CommitWith => new(MockFileSystem);

    [SetUp]
    public void Setup()
    {
        MockFileSystem = new MockFileSystem();
    }

    protected static Linter GetSubject()
    {
        return new Linter();
    }

    protected string GetCommitConfig(
        bool maxSubjectLengthEnabled = true,
        int maxSubjectLengthValue = 90,
        bool conventionalCommitEnabled = true,
        List<string>? conventionalCommitTypes = null
    )
    {
        var config = new CommitMessageConfigRoot
        {
            Config = new CommitMessageConfig
            {
                MaxSubjectLength = new MaxSubjectLength
                {
                    Enabled = maxSubjectLengthEnabled,
                    Value = maxSubjectLengthValue,
                },
                ConventionalCommit = new ConventionalCommitConfig
                {
                    Enabled = conventionalCommitEnabled,
                    Types =
                        conventionalCommitTypes
                        ??
                        [
                            "feat",
                            "fix",
                            "refactor",
                            "build",
                            "chore",
                            "style",
                            "test",
                            "docs",
                            "perf",
                            "revert",
                        ],
                },
            },
        };

        const string commitMessageConfigFileName = "commit-message-config.json";
        MockFileSystem.File.WriteAllText(
            commitMessageConfigFileName,
            JsonSerializer.Serialize(
                config,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower }
            )
        );
        return commitMessageConfigFileName;
    }

    protected class CommitMessage(IFileSystem fileSystem)
    {
        internal string SubjectWithTypeAndDescription =>
            "feat: description".ToCommitFile(fileSystem);

        public string BodyWithout1BlankLineFromSubject =>
            """
                feat: description
                body
                """.ToCommitFile(fileSystem);

        public string BodyWith1BlankLineFromSubject =>
            """
                feat: description

                body
                """.ToCommitFile(fileSystem);

        public string EmptyBodyWith1BlankLineFromSubject =>
            """
                feat: description



                """.ToCommitFile(fileSystem);
    }
}
