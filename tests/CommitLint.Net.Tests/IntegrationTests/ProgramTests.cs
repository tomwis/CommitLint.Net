using System.Text;
using FluentAssertions;

namespace CommitLint.Net.Tests.IntegrationTests;

[TestFixture]
public class ProgramTests
{
    private TextWriter _originalConsoleOut;

    [SetUp]
    public void SetUp()
    {
        _originalConsoleOut = Console.Out;
    }

    [TearDown]
    public void TearDown()
    {
        Console.SetOut(_originalConsoleOut);
    }

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
    public void WhenMainIsCalledWithInvalidCommitMessage_ThenReturnOne()
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
        var result = Program.Main(args);

        // Assert
        result.Should().Be(1);
    }

    [Test]
    public void WhenMainIsCalledWithInvalidConfigFile_ThenReturnOne()
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
        var result = Program.Main(args);

        // Assert
        result.Should().Be(1);
    }

    [Test]
    public void WhenMainIsCalledWithEmptyConfigFile_ThenUseDefaultConfig()
    {
        // Arrange
        string[] args = ["--commit-file", "IntegrationTests/Data/sampleCommit.txt"];

        // Act
        var result = Program.Main(args);

        // Assert
        result.Should().Be(0);
    }

    [Test]
    public void WhenLinterThrowsCommitFormatException_ThenMainReturnsOne()
    {
        // Arrange
        string[] args = ["--commit-file", "IntegrationTests/Data/sampleInvalidCommit.txt"];

        // Act
        var result = Program.Main(args);

        // Assert
        result.Should().Be(1);
    }

    [Test]
    public void WhenLinterThrowsAnyOtherExceptionThanCommitFormatException_ThenMainReturnsOne()
    {
        // Arrange
        string[] args = ["--commit-file", "IntegrationTests/Data/nonExistingCommit.txt"];

        // Act
        var result = Program.Main(args);

        // Assert
        result.Should().Be(1);
    }

    [Test]
    public void WhenLinterThrowsCommitFormatException_ThenPrintExceptionMessage()
    {
        // Arrange
        const string expectedMessage = "Commit message is in invalid format. Error: ";
        var stringBuilder = new StringBuilder();
        Console.SetOut(new StringWriter(stringBuilder));
        string[] args = ["--commit-file", "IntegrationTests/Data/sampleInvalidCommit.txt"];

        // Act
        _ = Program.Main(args);

        // Assert
        stringBuilder
            .ToString()
            .TrimEnd()
            .Split(Environment.NewLine)
            .Last()
            .Should()
            .StartWith(expectedMessage);
    }

    [Test]
    public void WhenLinterThrowsCommitFormatException_ThenDoNotPrintExceptionStackTrace()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        Console.SetOut(new StringWriter(stringBuilder));
        string[] args = ["--commit-file", "IntegrationTests/Data/sampleInvalidCommit.txt"];

        // Act
        _ = Program.Main(args);

        // Assert
        stringBuilder
            .ToString()
            .ToLower()
            .Should()
            .NotContain("exception")
            .And.NotContain("   at ")
            .And.NotContain(":line ");
    }

    [Test]
    public void WhenLinterThrowsAnyOtherExceptionThanCommitFormatException_ThenDoNotPrintExceptionStackTrace()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        Console.SetOut(new StringWriter(stringBuilder));
        string[] args = ["--commit-file", "IntegrationTests/Data/nonExistingCommit.txt"];

        // Act
        _ = Program.Main(args);

        // Assert
        stringBuilder
            .ToString()
            .ToLower()
            .Should()
            .NotContain("exception")
            .And.NotContain("   at ")
            .And.NotContain(":line ");
    }
}
