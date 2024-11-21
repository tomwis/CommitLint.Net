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
                var linterConfig = new LinterConfig(
                    o.CommitMessageFileName,
                    o.CommitMessageConfigFileName
                );
                new Linter().Run(linterConfig);
            })
            .WithNotParsed(errors =>
            {
                exitCode = 1;
            });

        return exitCode;
    }
}
