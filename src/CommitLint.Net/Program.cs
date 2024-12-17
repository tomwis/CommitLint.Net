using CommandLine;
using CommitLint.Net.Logging;

namespace CommitLint.Net;

public class Program
{
    public static int Main(string[] args)
    {
        int exitCode = 0;

        Parser
            .Default.ParseArguments<Options>(args)
            .WithParsed(o =>
            {
                try
                {
                    exitCode = SetupLogLevel(o);
                    if (exitCode == 1)
                    {
                        return;
                    }

                    var configFileName = o.CommitMessageConfigFileName;
                    if (string.IsNullOrWhiteSpace(configFileName))
                    {
                        Log.Info("No config file provided. Using default config.");
                        configFileName = GetDefaultConfig();
                    }

                    var linterConfig = new LinterConfig(o.CommitMessageFileName, configFileName);
                    var linter = new Linter();
                    linter.Run(linterConfig);
                }
                catch (CommitFormatException ex)
                {
                    Log.Error(ex.Message);
                    exitCode = 1;
                }
                catch (Exception)
                {
                    Log.Error("Unknown error.");
                    exitCode = 1;
                }
            })
            .WithNotParsed(errors =>
            {
                exitCode = 1;
            });

        return exitCode;
    }

    private static int SetupLogLevel(Options o)
    {
        var exitCode = 0;
        if (o.Verbosity is null)
            return exitCode;

        if (Enum.TryParse(typeof(Log.LogLevel), o.Verbosity, true, out var result))
        {
            Log.MinLogLevel = (Log.LogLevel)result;
        }
        else
        {
            exitCode = 1;
            var helpText = typeof(Options)
                .GetProperty(nameof(Options.Verbosity))
                ?.GetCustomAttributes(typeof(OptionAttribute), false)
                .OfType<OptionAttribute>()
                .FirstOrDefault()
                ?.HelpText;

            Log.Error($"Verbosity option was used with unsupported value. Usage: {helpText}");
        }

        return exitCode;
    }

    private static string GetDefaultConfig()
    {
        return Path.Combine(
            AppContext.BaseDirectory,
            "content",
            "config",
            "commit-message-config-default.json"
        );
    }
}
