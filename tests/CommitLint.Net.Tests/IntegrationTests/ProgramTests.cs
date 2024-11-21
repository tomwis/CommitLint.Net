using System;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Text.Json;
using CommandLine;
using CommitLint.Net;
using CommitLint.Net.Models;
using FluentAssertions;
using NUnit.Framework;

namespace CommitLint.Net.Tests.IntegrationTests;

[TestFixture]
public class ProgramTests
{
    [Test]
    public void WhenMainIsCalledWithValidArguments_ThenReturnZero()
    {
        // Arrange
        string[] args =
        [
            "--commit-file",
            "IntegrationTests/Data/sampleCommit.txt",
            "--commit-message-config-file",
            "IntegrationTests/Data/sampleConfig.json",
        ];

        // Act
        var result = Program.Main(args);

        // Assert
        result.Should().Be(0);
    }

    [Test]
    public void WhenMainIsCalledWithInvalidCommitMessage_ThenThrowCommitFormatException()
    {
        // Arrange
        string[] args =
        [
            "--commit-file",
            "IntegrationTests/Data/sampleInvalidCommit.txt",
            "--commit-message-config-file",
            "IntegrationTests/Data/sampleConfig.json",
        ];

        // Act
        var action = () => Program.Main(args);

        // Assert
        action.Should().Throw<CommitFormatException>();
    }

    [Test]
    public void WhenMainIsCalledWithInvalidConfigFile_ThenThrowFileNotFoundException()
    {
        // Arrange
        string[] args =
        [
            "--commit-file",
            "IntegrationTests/Data/sampleCommit.txt",
            "--commit-message-config-file",
            "nonExistentConfig.json",
        ];

        // Act
        var action = () => Program.Main(args);

        // Assert
        action.Should().Throw<FileNotFoundException>();
    }
}
