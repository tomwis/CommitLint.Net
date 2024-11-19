using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.LinterRulesTests;

public class BlankLineBetweenSubjectAndBodyTests : LinterRulesTestsBase
{
    [Test]
    public void When_CC_CommitMessageHasBodyWithout1BlankLineFromSubject_ThenThrowCommitFormatException()
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            CommitWith.BodyWithout1BlankLineFromSubject,
            GetCommitConfig(),
            MockFileSystem
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().Throw<CommitFormatException>();
    }

    [Test]
    public void When_NotCC_CommitMessageHasBodyWithout1BlankLineFromSubject_ThenDoNotThrowCommitFormatException()
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            CommitWith.BodyWithout1BlankLineFromSubject,
            GetCommitConfig(conventionalCommitEnabled: false),
            MockFileSystem
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().NotThrow<CommitFormatException>();
    }

    [Test]
    public void When_CC_CommitMessageHasBodyWith1BlankLineFromSubject_ThenDoNotThrowCommitFormatException()
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            CommitWith.BodyWith1BlankLineFromSubject,
            GetCommitConfig(),
            MockFileSystem
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().NotThrow<CommitFormatException>();
    }
}
