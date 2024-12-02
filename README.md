## What is CommitLint.Net?

It is a dotnet tool for validating commit messages according to [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) specification. It can be used in git hook, for example with [Husky.NET](https://alirezanet.github.io/Husky.Net/). It is also included in [xpd](https://github.com/tomwis/xpd) project.

## How to use

### Install
```
dotnet new tool-manifest # if this is your first local dotnet tool in this project
dotnet tool install CommitLint.Net
```

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
        "${args}"
      ]
    }
  ]
}
```


### Custom config

CommitLint.Net uses default configuration which is the following:
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

If you want to change it, create json file, e.g. `commit-message-config.json` with above content and modify options as needed. Options listed above are all currently supported options.

If you're using Husky, then you'll also need to add this custom config to `task-runner.json` with `--commit-message-config-file` option:

```
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
```
`path/to/commit-message-config.json` is relative to root of your repo.


### Direct usage

If you'd like to use it directly, without Husky:
```
dotnet commit-lint --commit-file "path/to/commit-message.txt" --commit-message-config-file "path/to/commit-message-config.json"
```