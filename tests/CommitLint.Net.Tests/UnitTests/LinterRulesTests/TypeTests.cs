using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Text.Json;
using CommitLint.Net.Models;
using CommitLint.Net.Tests.UnitTests.Extensions;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.LinterRulesTests;

public class TypeTests : LinterRulesTestsBase
{
    [TestCase("feat")]
    [TestCase("fix")]
    [TestCase("refactor")]
    public void When_CC_CommitMessageStartsWithAllowedType_ThenDoNotThrowCommitFormatException(
        string type
    )
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            $"{type}: description".ToCommitFile(MockFileSystem),
            GetCommitConfig(conventionalCommitTypes: ["feat", "fix", "refactor"]),
            MockFileSystem
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().NotThrow<CommitFormatException>();
    }

    [TestCase("some")]
    [TestCase("wrong")]
    [TestCase("types")]
    public void When_CC_CommitMessageStartsWithNotAllowedType_ThenThrowCommitFormatException(
        string type
    )
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            $"{type}: description".ToCommitFile(MockFileSystem),
            GetCommitConfig(conventionalCommitTypes: ["feat", "fix", "refactor"]),
            MockFileSystem
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().Throw<CommitFormatException>();
    }

    [TestCase("some")]
    [TestCase("wrong")]
    [TestCase("types")]
    public void When_CC_CommitMessageStartsWithNotAllowedType_AndCheckIsDisabled_ThenDoNotThrowCommitFormatException(
        string type
    )
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            $"{type}: description".ToCommitFile(MockFileSystem),
            GetCommitConfig(
                conventionalCommitEnabled: false,
                conventionalCommitTypes: ["feat", "fix", "refactor"]
            ),
            MockFileSystem
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().NotThrow<CommitFormatException>();
    }

    [TestCase(":")]
    [TestCase(" ")]
    public void When_CC_CommitMessageTypeIsNotFollowedByColonAndSpace_ThenThrowCommitFormatException(
        string separator
    )
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            $"feat{separator}description".ToCommitFile(MockFileSystem),
            GetCommitConfig(),
            MockFileSystem
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().Throw<CommitFormatException>();
    }

    [TestCase(":")]
    [TestCase(" ")]
    public void When_NotCC_CommitMessageTypeIsNotFollowedByColonAndSpace_ThenDoNotThrowCommitFormatException(
        string separator
    )
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            $"feat{separator}description".ToCommitFile(MockFileSystem),
            GetCommitConfig(conventionalCommitEnabled: false),
            MockFileSystem
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().NotThrow<CommitFormatException>();
    }
}
