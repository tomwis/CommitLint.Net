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
