using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Text.Json;
using CommitLint.Net.Models;
using CommitLint.Net.Tests.UnitTests.Extensions;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests;

public class LinterTests
{
    private IFileSystem _mockFileSystem;

    [SetUp]
    public void Setup()
    {
        _mockFileSystem = new MockFileSystem();
    }

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
    public void WhenCommitMessageSubjectIsLongerThanMaxAllowed_ThenThrowCommitFormatException(
        string message,
        int maxLength
    )
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            message.ToCommitFile(_mockFileSystem),
            GetCommitConfig(maxSubjectLengthValue: maxLength),
            _mockFileSystem
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
    public void WhenCommitMessageSubjectIsNotLongerThanMaxAllowed_ThenDoNotThrowCommitFormatException(
        string message,
        int maxLength
    )
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            message.ToCommitFile(_mockFileSystem),
            GetCommitConfig(maxSubjectLengthValue: maxLength),
            _mockFileSystem
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().NotThrow<CommitFormatException>();
    }

    [Test]
    public void WhenCommitMessageSubjectLengthCheckIsDisabled_AndSubjectIsLongerThanMaxAllowed_ThenDoNotThrowCommitFormatException()
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            CommitWith.SubjectWithTypeAndDescription,
            GetCommitConfig(maxSubjectLengthEnabled: false, maxSubjectLengthValue: 1),
            _mockFileSystem
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().NotThrow<CommitFormatException>();
    }

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
            $"{type}: description".ToCommitFile(_mockFileSystem),
            GetCommitConfig(conventionalCommitTypes: ["feat", "fix", "refactor"]),
            _mockFileSystem
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
            $"{type}: description".ToCommitFile(_mockFileSystem),
            GetCommitConfig(conventionalCommitTypes: ["feat", "fix", "refactor"]),
            _mockFileSystem
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
            $"{type}: description".ToCommitFile(_mockFileSystem),
            GetCommitConfig(
                conventionalCommitEnabled: false,
                conventionalCommitTypes: ["feat", "fix", "refactor"]
            ),
            _mockFileSystem
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
            $"feat{separator}description".ToCommitFile(_mockFileSystem),
            GetCommitConfig(),
            _mockFileSystem
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
            $"feat{separator}description".ToCommitFile(_mockFileSystem),
            GetCommitConfig(conventionalCommitEnabled: false),
            _mockFileSystem
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().NotThrow<CommitFormatException>();
    }

    [Test]
    public void When_CC_CommitMessageHasEmptyDescription_ThenThrowCommitFormatException()
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            "feat: ".ToCommitFile(_mockFileSystem),
            GetCommitConfig(),
            _mockFileSystem
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
            "feat: ".ToCommitFile(_mockFileSystem),
            GetCommitConfig(conventionalCommitEnabled: false),
            _mockFileSystem
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().NotThrow<CommitFormatException>();
    }

    [Test]
    public void When_CC_CommitMessageHasBodyWithout1BlankLineFromSubject_ThenThrowCommitFormatException()
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            CommitWith.BodyWithout1BlankLineFromSubject,
            GetCommitConfig(),
            _mockFileSystem
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
            _mockFileSystem
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
            _mockFileSystem
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().NotThrow<CommitFormatException>();
    }

    [Test]
    public void When_CC_CommitMessageHasBody_AndItIsEmpty_ThenThrowCommitFormatException()
    {
        // Arrange
        var subject = GetSubject();
        var linterConfig = new LinterConfig(
            CommitWith.EmptyBodyWith1BlankLineFromSubject,
            GetCommitConfig(),
            _mockFileSystem
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
            _mockFileSystem
        );

        // Act
        var func = () => subject.Run(linterConfig);

        // Assert
        func.Should().NotThrow<CommitFormatException>();
    }

    private static Linter GetSubject()
    {
        return new Linter();
    }

    private string GetCommitConfig(
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
        _mockFileSystem.File.WriteAllText(
            commitMessageConfigFileName,
            JsonSerializer.Serialize(
                config,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower }
            )
        );
        return commitMessageConfigFileName;
    }

    private CommitMessage CommitWith => new(_mockFileSystem);

    private class CommitMessage(IFileSystem fileSystem)
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
