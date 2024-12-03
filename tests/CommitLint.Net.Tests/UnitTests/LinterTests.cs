using System.IO.Abstractions.TestingHelpers;
using System.Text.Json;
using CommitLint.Net.JsonNamingPolicies;
using CommitLint.Net.Models;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests;

[TestFixture]
public class LinterTests
{
    private MockFileSystem _mockFileSystem;

    [SetUp]
    public void SetUp()
    {
        _mockFileSystem = new MockFileSystem();
    }

    [Test]
    public void WhenConfigIsValid_AndCommitMessageIsCorrect_ThenDoesNotThrowException()
    {
        // Arrange
        const string validCommitMessage = "feat: add new feature";
        var subject = new Linter();
        var linterConfig = CreateLinterConfig(validCommitMessage);

        // Act
        var act = () => subject.Run(linterConfig);

        // Assert
        act.Should().NotThrow<CommitFormatException>();
    }

    [Test]
    public void WhenConfigIsValid_AndCommitMessageIsEmpty_ThenThrowsCommitFormatException()
    {
        // Arrange
        const string emptyCommitMessage = "";
        var subject = new Linter();
        var linterConfig = CreateLinterConfig(emptyCommitMessage);

        // Act
        var act = () => subject.Run(linterConfig);

        // Assert
        act.Should().Throw<CommitFormatException>();
    }

    [Test]
    public void WhenConfigIsValid_AndCommitMessageIsIncorrect_ThenThrowsCommitFormatException()
    {
        // Arrange
        const string invalidCommitMessage = "invalid commit format";
        var subject = new Linter();
        var linterConfig = CreateLinterConfig(invalidCommitMessage);

        // Act
        var act = () => subject.Run(linterConfig);

        // Assert
        act.Should().Throw<CommitFormatException>();
    }

    private string GetCommitConfig()
    {
        var config = new CommitMessageConfigRoot
        {
            Config = new CommitMessageConfig
            {
                MaxSubjectLength = new MaxSubjectLength { Enabled = true, Value = 90 },
                ConventionalCommit = new ConventionalCommitConfig
                {
                    Enabled = true,
                    Types = new List<string>
                    {
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
                    },
                },
            },
        };

        const string commitMessageConfigFileName = "commit-message-config.json";
        _mockFileSystem.File.WriteAllText(
            commitMessageConfigFileName,
            JsonSerializer.Serialize(
                config,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = new KebabCaseLowerNamingPolicy(),
                }
            )
        );
        return commitMessageConfigFileName;
    }

    private LinterConfig CreateLinterConfig(string commitMessage)
    {
        const string commitMessageFileName = "commit.txt";
        _mockFileSystem.File.WriteAllText(commitMessageFileName, commitMessage);
        var commitMessageConfigFileName = GetCommitConfig();
        return new LinterConfig(
            commitMessageFileName,
            commitMessageConfigFileName,
            _mockFileSystem
        );
    }
}
