using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests;

public class LinterTests
{
    [Test]
    public void CommitMessageWithCorrectSubject()
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            "TestFiles/commit.txt",
            "TestFiles/commit-message-config.json"
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().NotThrow();
    }

    [Test]
    public void CommitMessageWithBody()
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            "TestFiles/commit-with-body.txt",
            "TestFiles/commit-message-config.json"
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().NotThrow();
    }

    private static Linter GetSubject()
    {
        return new Linter();
    }
}
