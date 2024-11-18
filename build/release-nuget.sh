#!/bin/bash

. config/.env
WORKING_DIR="src/CommitLint.Net"
CS_PROJ_FILE="$WORKING_DIR/CommitLint.Net.csproj"
NUGET_SOURCE="https://api.nuget.org/v3/index.json"

get_version() {
    xmllint --xpath "string(//Version)" "$CS_PROJ_FILE"
}

get_package_name() {
    xmllint --xpath "string(//PackageId)" "$CS_PROJ_FILE"
}

print_error() {
  local message="$1"
  echo -e "\033[31m$message\033[0m"
}

underline() {
  local message="$1"
  echo "$(tput smul)$message$(tput rmul)"
}

prepare() {
    echo "Incrementing version and generating changelog with versionize..."

    if ! dotnet versionize --workingDir "$WORKING_DIR" --skip-commit --skip-tag --exit-insignificant-commits; then
        echo -n "There are no feat or fix commits since last release. Changelog will be empty. Continue anyway? [y/$(underline "n")] "
        read -r response
        if [[ "${response:l}" =~ ^(yes|y)$ ]]; then
            echo "Continuing..."
            dotnet versionize --workingDir "$WORKING_DIR" --skip-commit --skip-tag
        else
            echo "Exiting without increasing version."
            exit 1
        fi
    fi
}

publish() {
    VERSION=$(get_version)

    if [ -z "$(git ls-files --others --exclude-standard)" ] && git diff --quiet --exit-code && git diff --quiet --cached --exit-code; then
        echo -n "
There are no changes to commit.
Usually you should run prepare command before publish to increment version number and generate changelog.
Are you sure you want to publish?
Version tag will be added to the last commit. [y/$(underline "n")] "

        read -r response
        if [[ -z "$response" || "${response:l}" =~ ^(no|n)$ ]]; then
            echo "Exiting without publishing."
            exit 1
        fi
    else
        echo "Committing changes..."
        git add .
        git commit -m "chore(release): $VERSION" --no-verify
    fi

    echo "Tagging commit with version 'v$VERSION'..."
    if [ "$(git tag --list "v$VERSION")" ]; then
        print_error "Tag already exists. Exiting."
        exit 1
    else
        git tag "v$VERSION"
    fi

    echo "Packing the project in Release mode..."
    dotnet pack "$WORKING_DIR" --configuration Release --verbosity quiet

    echo "Publishing to NuGet..."
    if [ -z "$NUGET_API_KEY" ]; then
        print_error "NUGET_API_KEY is empty. Set it in config/.env file."
        exit 1
    fi

    echo -n "All ready. Publish? [$(underline "y")/n] "

    read -r response
    if [[ "${response:l}" =~ ^(no|n)$ ]]; then
        echo "Exiting without publishing."
        exit 1
    fi
    PACKAGE_NAME=$(get_package_name)
    dotnet nuget push "$WORKING_DIR"/nupkg/"$PACKAGE_NAME"."$VERSION".nupkg --api-key "$NUGET_API_KEY" --source "$NUGET_SOURCE"

    echo "Published NuGet package: https://www.nuget.org/packages/$PACKAGE_NAME/$VERSION"
}

case "$1" in
    prepare)
        prepare
        ;;
    publish)
        publish
        ;;
    *)

        description="
Usage: $0 {prepare|publish}

Subcommands:
    prepare     Prepares changelog and new version number.
                   - Increments the version in the .csproj file. <Version> tag is required.
                   - Generates a changelog for given version in CHANGELOG.md
                      - File is located in the project's directory.
                      - Changes are taken from commit messages since the previous version tag. Requires conventional commits.
                      - Changelog is generated only if there are feat or fix commits since the last release (Otherwise changelog is empty).

    publish     Publishes to NuGet, saves changes from ""prepare"" and adds tag.
                   - Commits current working directory
                   - Adds version tag
                   - Packs the project in Release mode
                   - Publishes to NuGet (Nuget API key is required in config/.env file)
"
        echo "$description"
        exit 1
        ;;
esac