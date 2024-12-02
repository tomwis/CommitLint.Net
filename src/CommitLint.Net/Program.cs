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
                var configFileName = o.CommitMessageConfigFileName ?? GetDefaultConfig();
                var linterConfig = new LinterConfig(o.CommitMessageFileName, configFileName);
                new Linter().Run(linterConfig);
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
