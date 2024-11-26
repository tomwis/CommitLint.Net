## What is CommitLint.Net?

It is a dotnet tool for validating commit messages according to [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) specification. It can be used in git hook, for example with [Husky.NET](https://alirezanet.github.io/Husky.Net/). It is also included in [xpd](https://github.com/tomwis/xpd) project.

## How to use

### Install
```
dotnet new tool-manifest # if this is your first local dotnet tool in this project
dotnet tool install CommitLint.Net
```

### Create config

Create file `commit-message-config.json` with following content:
```
{
    "config": {
        "max-subject-length": {
            "enabled": true,
            "value": 90
        },
        "conventional-commit": {
            "enabled": true,
            "types": [ "feat", "fix", "refactor", "build", "chore", "style", "test", "docs", "perf", "revert" ]
        }
    }
}
```

These are currently supported options.

### Configure Husky.NET

Install & configure Husky.NET:
```
dotnet tool install Husky.Net
dotnet husky install
dotnet husky add commit-msg -c "dotnet husky run --group commit-msg --args \"\$1\""
```

Then in `.husky/task-runner.json` add task to run linter:
```
{
  "$schema": "https://alirezanet.github.io/Husky.Net/schema.json",
  "tasks": [
    {
      "name": "commit-message-linter",
      "group": "commit-msg",
      "command": "dotnet",
      "args": [
        "commit-lint",
        "--commit-file",
        "${args}",
        "--commit-message-config-file",
        "path/to/commit-message-config.json"
      ]
    }
  ]
}
```
`path/to/commit-message-config.json` is relative to root of yout repo.

### Direct usage

If you'd like to use it directly, without husky:
```
dotnet commit-lint --commit-file "path/to/commit-message.txt" --commit-message-config-file "path/to/commit-message-config.json"
```