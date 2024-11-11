using NUnit.Framework;

namespace CommitLint.Net.Tests.IntegrationTests;

[SetUpFixture]
public class SetupFixture
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        if (Environment.GetEnvironmentVariable("GIT_HOOK_EXECUTION") == "true")
        {
            throw new InvalidOperationException("Integration tests shouldn't run in git hooks.");
        }
    }
}
