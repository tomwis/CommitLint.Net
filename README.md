## What is CommitLint.Net?

It is a dotnet tool for validating commit messages according to [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) specification. It can be used in git hook, for example with [Husky.NET](https://alirezanet.github.io/Husky.Net/). It is also included in [xpd](https://github.com/tomwis/xpd) project.

It doesn't have a full set of rules yet.

## How to use

Install globally:
```
dotnet tool install -g CommitLint.Net
```

or locally for the project only:
```
dotnet new tool-manifest # if this is your first local dotnet tool in this project
dotnet tool install CommitLint.Net
```

