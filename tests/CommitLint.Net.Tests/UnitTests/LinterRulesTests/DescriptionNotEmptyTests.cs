using CommitLint.Net.Tests.UnitTests.Extensions;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.LinterRulesTests;

public class DescriptionNotEmptyTests : LinterRulesTestsBase
{
    [Test]
    public void When_CC_CommitMessageHasEmptyDescription_ThenThrowCommitFormatException()
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            "feat: ".ToCommitFile(MockFileSystem),
            GetCommitConfig(),
            MockFileSystem
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().Throw<CommitFormatException>();
    }

    [Test]
    public void When_NotCC_CommitMessageHasEmptyDescription_ThenDoNotThrowCommitFormatException()
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            "feat: ".ToCommitFile(MockFileSystem),
            GetCommitConfig(conventionalCommitEnabled: false),
            MockFileSystem
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().NotThrow<CommitFormatException>();
    }
}
