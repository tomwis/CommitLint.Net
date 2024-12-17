namespace CommitLint.Net.Logging;

public static class Log
{
    public static LogLevel MinLogLevel { get; set; } = LogLevel.Info;

    public static void Error(string message)
    {
        LogInternal(message, LogLevel.Error);
    }

    public static void Info(string message)
    {
        LogInternal(message, LogLevel.Info);
    }

    public static void Verbose(string message)
    {
        LogInternal(message, LogLevel.Verbose);
    }

    private static void LogInternal(string message, LogLevel logLevel)
    {
        if (logLevel > MinLogLevel)
            return;

        Console.WriteLine(message);
    }

    public enum LogLevel
    {
        Error,
        Info,
        Verbose,
    }
}
