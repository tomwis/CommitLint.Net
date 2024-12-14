## What is CommitLint.Net?

It is a dotnet tool for validating commit messages according to [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) specification. It can be used in git hook, for example with [Husky.NET](https://alirezanet.github.io/Husky.Net/). It is also included in [xpd](https://github.com/tomwis/xpd) project.

## How to use

### Install
```shell
dotnet new tool-manifest # if this is your first local dotnet tool in this project
dotnet tool install CommitLint.Net
```

### Configure Husky.NET

Install & configure Husky.NET (there is slight difference between platform in parsing $1 argument):

#### MacOS
```shell
dotnet tool install Husky
dotnet husky install
dotnet husky add commit-msg -c "dotnet husky run --group commit-msg --args \"\$1\""
```

#### Windows
```shell
dotnet tool install Husky
dotnet husky install
dotnet husky add commit-msg -c 'dotnet husky run --group commit-msg --args """$1"""'
```

In the end, entry in `commit-msg` file that will be created by `dotnet husky add` should be:
```shell
dotnet husky run --group commit-msg --args "$1"
```

Then in `.husky/task-runner.json` add task to run linter:
```json
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
```json
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

```json
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
```shell
dotnet commit-lint --commit-file "path/to/commit-message.txt" --commit-message-config-file "path/to/commit-message-config.json"
```


# Current Rules

1. Max subject length check
2. Scope format check
3. Type format check + if it is on allowed list
4. Description not empty check
5. Blank line before body check
6. Body not empty check
7. Blank line before footers check
8. Optional "!" character after type/scope for breaking change check
9. Breaking change token in footer format check
10. Footers content not empty check

Rules 2 - 10 are
from [Conventional Commits specification](https://www.conventionalcommits.org/en/v1.0.0/#specification). Rule 1 is an additional one.

Rule 1 can be disabled in config and rules 2 - 10 can be disabled in config as well, but all together, not one by one.