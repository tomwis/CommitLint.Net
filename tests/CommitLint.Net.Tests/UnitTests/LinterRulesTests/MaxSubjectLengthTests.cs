using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Text.Json;
using CommitLint.Net.Models;
using CommitLint.Net.Tests.UnitTests.Extensions;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.LinterRulesTests;

public class MaxSubjectLengthTests : LinterRulesTestsBase
{
    [TestCase("fix: 1", 5)]
    [TestCase("feat: description", 10)]
    [TestCase("feat: description", 16)]
    [TestCase(
        "feat: this is a feature commit that adds some functionality and has subject line that is longer than max allowed",
        90
    )]
    [TestCase(
        "feat: this is a feature commit that adds some functionality and has subject line that is longer than max allowed",
        111
    )]
    public void WhenSubjectIsLongerThanMaxAllowed_ThenThrowCommitFormatException(
        string message,
        int maxLength
    )
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            message.ToCommitFile(MockFileSystem),
            GetCommitConfig(maxSubjectLengthValue: maxLength),
            MockFileSystem
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().Throw<CommitFormatException>();
    }

    [TestCase("fix: 1", 6)]
    [TestCase("feat: description", 17)]
    [TestCase("feat: description", 18)]
    [TestCase(
        "feat: this is a feature commit that adds some functionality and has subject line that is longer than max allowed",
        112
    )]
    [TestCase(
        "feat: this is a feature commit that adds some functionality and has subject line that is longer than max allowed",
        113
    )]
    public void WhenSubjectIsNotLongerThanMaxAllowed_ThenDoNotThrowCommitFormatException(
        string message,
        int maxLength
    )
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            message.ToCommitFile(MockFileSystem),
            GetCommitConfig(maxSubjectLengthValue: maxLength),
            MockFileSystem
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().NotThrow<CommitFormatException>();
    }

    [Test]
    public void WhenRuleIsDisabled_AndSubjectIsLongerThanMaxAllowed_ThenDoNotThrowCommitFormatException()
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            CommitWith.SubjectWithTypeAndDescription,
            GetCommitConfig(maxSubjectLengthEnabled: false, maxSubjectLengthValue: 1),
            MockFileSystem
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().NotThrow<CommitFormatException>();
    }
}
