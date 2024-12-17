using System.Linq.Expressions;
using CommitLint.Net.Logging;
using FluentAssertions;

namespace CommitLint.Net.Tests.UnitTests.LoggingTests;

[TestFixture]
public class LogTests
{
    private StringWriter _consoleOutput;
    private TextWriter _originalConsoleOut;

    [SetUp]
    public void SetUp()
    {
        _originalConsoleOut = Console.Out;
        _consoleOutput = new StringWriter();
        Console.SetOut(_consoleOutput);
    }

    [TearDown]
    public void TearDown()
    {
        Console.SetOut(_originalConsoleOut);
        _consoleOutput.Dispose();
    }

    [TestCaseSource(nameof(LogLevelsEqualMinLogLevel))]
    public void WhenMessageIsLoggedWithLogLevelEqualToMinLogLevel_ThenMessageIsLogged(
        Log.LogLevel minLogLevel,
        Expression<Action<string>> logAction,
        string expectedMessage
    )
    {
        // Arrange
        Log.MinLogLevel = minLogLevel;
        var action = logAction.Compile();

        // Act
        action(expectedMessage);

        // Assert
        _consoleOutput.ToString().Should().Be(expectedMessage + Environment.NewLine);
    }

    [TestCaseSource(nameof(LogLevelsAboveMinLogLevel))]
    public void WhenMessageIsLoggedWithLogLevelAboveMinLogLevel_ThenMessageIsNotLogged(
        Log.LogLevel minLogLevel,
        Expression<Action<string>> logAction
    )
    {
        // Arrange
        Log.MinLogLevel = minLogLevel;
        var action = logAction.Compile();

        // Act
        action("some message that should not be logged");

        // Assert
        _consoleOutput.ToString().Should().BeEmpty();
    }

    [TestCaseSource(nameof(LogLevelsBelowMinLogLevel))]
    public void WhenMessageIsLoggedWithLogLevelBelowMinLogLevel_ThenMessageIsLogged(
        Log.LogLevel minLogLevel,
        Expression<Action<string>> logAction,
        string expectedMessage
    )
    {
        // Arrange
        Log.MinLogLevel = minLogLevel;
        var action = logAction.Compile();

        // Act
        action(expectedMessage);

        // Assert
        _consoleOutput.ToString().Should().Be(expectedMessage + Environment.NewLine);
    }

    private static IEnumerable<LogWithMessageTestCaseData> LogLevelsEqualMinLogLevel()
    {
        yield return new(Log.LogLevel.Error, message => Log.Error(message), "message1");
        yield return new(Log.LogLevel.Info, message => Log.Info(message), "message2");
        yield return new(Log.LogLevel.Verbose, message => Log.Verbose(message), "message3");
    }

    private static IEnumerable<LogTestCaseData> LogLevelsAboveMinLogLevel()
    {
        yield return new(Log.LogLevel.Error, message => Log.Info(message));
        yield return new(Log.LogLevel.Error, message => Log.Verbose(message));
        yield return new(Log.LogLevel.Info, message => Log.Verbose(message));
    }

    private static IEnumerable<LogWithMessageTestCaseData> LogLevelsBelowMinLogLevel()
    {
        yield return new(Log.LogLevel.Info, message => Log.Error(message), "message7");
        yield return new(Log.LogLevel.Verbose, message => Log.Error(message), "message8");
        yield return new(Log.LogLevel.Verbose, message => Log.Info(message), "message9");
    }

    private class LogWithMessageTestCaseData(
        Log.LogLevel minLogLevel,
        Expression<Action<string>> logAction,
        string? expectedMessage = null
    ) : TestCaseData(minLogLevel, logAction, expectedMessage)
    {
        public Log.LogLevel MinLogLevel { get; } = minLogLevel;
        public Expression<Action<string>> LogAction { get; } = logAction;
        public string? ExpectedMessage { get; } = expectedMessage;
    }

    private class LogTestCaseData(
        Log.LogLevel minLogLevel,
        Expression<Action<string>> logAction,
        string? expectedMessage = null
    ) : TestCaseData(minLogLevel, logAction)
    {
        public Log.LogLevel MinLogLevel { get; } = minLogLevel;
        public Expression<Action<string>> LogAction { get; } = logAction;
    }
}
