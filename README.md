## Project Initialization

This project has been initialized with the following features:

- Structure:
```
    Project Root 'CommitLint.Net'
    ├── .config/    # Dotnet tools config folder
    ├── .git/       # New git repo is initialized
    ├── .husky/
    ├── src/
    │   └── Project 'CommitLint.Net'
    ├── tests/
    │   └── Tests Project 'CommitLint.Net.Tests'
    │       └── UnitTests
    │       └── IntegrationTests
    │           └── SetupFixture.cs
    │   └── Convention Tests Project 'CommitLint.Net.ConventionTests'
    ├── config/
    │   └── Cache of Husky restore
    ├── build/
    ├── docs/
    ├── samples/
    ├── CommitLint.Net.sln
    ├── README.md
    ├── .gitignore  # Default .gitignore from dotnet CLI wit hsome additions
    ├── .editorconfig
    ├── Directory.Build.targets
    └── Directory.Packages.props
```
- Solution contains solution folder "SolutionSettings" with files:
  - .gitignore
  - [Directory.Build.targets](https://learn.microsoft.com/en-us/visualstudio/msbuild/customize-by-directory)
  - [Directory.Packages.props](https://devblogs.microsoft.com/nuget/introducing-central-package-management/)
  - .husky/task-runner.json
- Nuget packages added:
    - Tests Project 'CommitLint.Net.Tests'
      - [AutoFixture](https://autofixture.github.io)
      - [AutoFixture.AutoNSubstitute](https://github.com/AutoFixture/AutoFixture?tab=readme-ov-file#mocking-libraries)
      - [FluentAssertions](https://fluentassertions.com)
      - [FluentAssertions.Analyzers](https://github.com/fluentassertions/fluentassertions.analyzers)
      - Microsoft.NET.Test.Sdk
      - [NSubstitute](https://nsubstitute.github.io)
      - [NSubstitute.Analyzers.CSharp](https://nsubstitute.github.io/help/nsubstitute-analysers/)
      - [NUnit](https://nunit.org)
      - [NUnit3TestAdapter](https://docs.nunit.org/articles/vs-test-adapter/Index.html)
      - [NUnit.Analyzers](https://docs.nunit.org/articles/nunit-analyzers/NUnit-Analyzers.html)
      - [coverlet.collector](https://github.com/coverlet-coverage/coverlet)
    - Convention Tests Project 'CommitLint.Net.ConventionTests'
      - [FluentAssertions](https://fluentassertions.com)
      - [FluentAssertions.Analyzers](https://github.com/fluentassertions/fluentassertions.analyzers)
      - Microsoft.NET.Test.Sdk
      - [NUnit](https://nunit.org)
      - [NUnit3TestAdapter](https://docs.nunit.org/articles/vs-test-adapter/Index.html)
      - [NUnit.Analyzers](https://docs.nunit.org/articles/nunit-analyzers/NUnit-Analyzers.html)
      - [coverlet.collector](https://github.com/coverlet-coverage/coverlet)
- Dotnet tools:
    - [`Husky.Net`](https://alirezanet.github.io/Husky.Net/) is installed and pre-commit hook is initialized
    - [`Csharpier`](https://csharpier.com) is installed and added to pre-commit hook. Check plugins for different IDEs [here](https://csharpier.com/docs/Editors) to run CSharpier on file save.
- Git hooks:
  - [pre-commit] Code formatting of staged files with [Csharpier](https://csharpier.com)
  - [pre-commit] Run solution build
  - [pre-commit] Run unit tests from solution
  - Environment variable `GIT_HOOK_EXECUTION` is set to `true` for git hooks, so that we can detect if code is executing in a git hook
    - There is a check for this var in `tests/CommitLint.Net.Tests/IntegrationTests/SetupFixture.cs` to make sure integration tests don't run in a git hook. The reason for this is following. `dotnet test` with  `--filter FullyQualifiedName~.Tests.UnitTests` is used to run tests. Parameter values from parametrized tests  are included in FQN. If any integration test includes a parameter with value `.Tests.UnitTests`, then it will be run within this filter. Additional check for env var prevents this.
- `Directory.Build.targets` file is created with husky restore (so that developers don't have to do that manually after cloning repo)
- `Directory.Packages.props` file is created and set to manage nuget packages versions
