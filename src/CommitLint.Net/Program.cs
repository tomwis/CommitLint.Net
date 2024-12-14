using CommandLine;

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
                        Console.WriteLine("No config file provided. Using default config.");
                        configFileName = GetDefaultConfig();
                    }

                    var linterConfig = new LinterConfig(o.CommitMessageFileName, configFileName);
                    var linter = new Linter();
                    linter.Run(linterConfig);
                }
                catch (CommitFormatException ex)
                {
                    Console.WriteLine(ex.Message);
                    exitCode = 1;
                }
                catch (Exception)
                {
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
