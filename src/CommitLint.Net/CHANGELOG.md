# Change Log

All notable changes to this project will be documented in this file. See [versionize](https://github.com/versionize/versionize) for commit guidelines.

<a name="0.5.2"></a>
## [0.5.2](https://www.github.com/tomwis/CommitLint.Net/releases/tag/v0.5.2) (2024-12-14)

### Changes
* updated README for Windows Husky.NET setup ([67173aed](https://www.github.com/tomwis/CommitLint.Net/commit/67173aed))
* added project and repository metadata to csproj for NuGet ([52883574](https://www.github.com/tomwis/CommitLint.Net/commit/52883574))

<a name="0.5.1"></a>
## [0.5.1](https://www.github.com/tomwis/CommitLint.Net/releases/tag/v0.5.1) (2024-12-03)

### Changes

* removed .Net 7.0 support ([80f48f9](https://www.github.com/tomwis/CommitLint.Net/commit/80f48f9ac4002cc2fe04670113dc2c53a1775c3d))

<a name="0.5.0"></a>
## [0.5.0](https://www.github.com/tomwis/CommitLint.Net/releases/tag/v0.5.0) (2024-12-03)

### Features

* added .Net 7.0 support ([80f48f9](https://www.github.com/tomwis/CommitLint.Net/commit/80f48f9ac4002cc2fe04670113dc2c53a1775c3d))

<a name="0.4.0"></a>
## [0.4.0](https://www.github.com/tomwis/CommitLint.Net/releases/tag/v0.4.0) (2024-12-02)

### Features

* added default config for commit message validation ([186eabf](https://www.github.com/tomwis/CommitLint.Net/commit/186eabfd8f2f2d7fa46c10cc331978141cacecc7))

<a name="0.3.0"></a>
## [0.3.0](https://www.github.com/tomwis/CommitLint.Net/releases/tag/v0.3.0) (2024-11-26)

### Features

* added rule to check for blank line before footers ([e66c68a](https://www.github.com/tomwis/CommitLint.Net/commit/e66c68a616f56d1427a36766a4f19eef3e105f68))
* added rule to check for optional breaking change exclamation mark in subject ([d9567a0](https://www.github.com/tomwis/CommitLint.Net/commit/d9567a08a6752228de72328e84e86cf643a69d6c))
* added rule to check if footers content is not empty ([817395c](https://www.github.com/tomwis/CommitLint.Net/commit/817395c39bd246df98492cfc1f01ea6b54184bab))
* added rule to validate commit message scope format ([5637098](https://www.github.com/tomwis/CommitLint.Net/commit/5637098e528a042f11b5b93223ac1f05bceb1c3a))
* added rule to validate BREAKING CHANGE token in footer ([a164cf0](https://www.github.com/tomwis/CommitLint.Net/commit/a164cf09c51ca4821fc4b7345437f5d1cf9cb8b7))

### Bug Fixes

* added missing checks when cc option is disabled ([28581bf](https://www.github.com/tomwis/CommitLint.Net/commit/28581bf3b92ce527310eb431313730176224b068))

<a name="0.2.0"></a>
## [0.2.0](https://www.github.com/tomwis/CommitLint.Net/releases/tag/v0.2.0) (2024-11-18)

### Features

* added readme to nuget package ([c6280f8](https://www.github.com/tomwis/CommitLint.Net/commit/c6280f8cd47305b8c35b1d292e18f9ed2c818039))

<a name="0.1.0"></a>
## [0.1.0](https://www.github.com/tomwis/CommitLint.Net/releases/tag/v0.1.0) (2024-11-11)

### Features

* Commit message validation rules implemented ([4334b14](https://www.github.com/tomwis/CommitLint.Net/commit/4334b1428f32fc999c5ce52978bb7b21971b7a7f)):
  - Max subject length
  - Type
  - Description not empty
  - Blank line between subject and body
  - Body not empty

