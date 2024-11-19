using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.LinterRulesTests;

public class BodyNotEmptyTests : LinterRulesTestsBase
{
    [Test]
    public void When_CC_CommitMessageHasBody_AndItIsEmpty_ThenThrowCommitFormatException()
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            CommitWith.EmptyBodyWith1BlankLineFromSubject,
            GetCommitConfig(),
            MockFileSystem
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().Throw<CommitFormatException>();
    }

    [Test]
    public void When_NotCC_CommitMessageHasBody_AndItIsEmpty_ThenDoNotThrowCommitFormatException()
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            CommitWith.EmptyBodyWith1BlankLineFromSubject,
            GetCommitConfig(conventionalCommitEnabled: false),
            MockFileSystem
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().NotThrow<CommitFormatException>();
    }
}
