using CommandLine;

namespace CommitLint.Net;

public class Program
{
    public static void Main(string[] args)
    {
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
            .WithNotParsed(errors => { });
    }
}
