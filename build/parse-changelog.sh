set -e

version="$1"
changelog_file="$2"
current_version_header="## \[${version}\]"

get_line_number() {
    local grep_output="$1"
    local delimiter=":"
    local field=1
    echo "$grep_output" | cut -d$delimiter -f"$field"
}

trim_whitespace() {
  WHITESPACE_PATTERN='\s*$'
  local string="$1"
  echo "${string//$WHITESPACE_PATTERN/}"
}

get_end_line() {
    VERSION_PATTERN="^## \[
                       (?<version>
                         [0-9]+ \. [0-9]+ \. [0-9]+
                       )
                     \]$"

    all_version_headers=$(grep -n "$VERSION_PATTERN" $changelog_file)
    current_and_previous=$(echo "$all_version_headers" | grep -A1 "$current_version_header")
    previous_version=$(echo "$current_and_previous" | tail -1)
    local end_line
    end_line=$(get_line_number "$previous_version")

    # If end_line is empty or the same as start_line then just get the end of the file
    if [ -z "$end_line" ] || [ "$end_line" = "$start_line" ]; then
      end_line=$(wc -l < $changelog_file)
    else
      # Exclude the line of the next version header and html anchor
      end_line=$((end_line - 2))
    fi

    trim_whitespace "$end_line"
}

start_line=$(get_line_number "$(grep -n "$current_version_header" $changelog_file)")
end_line=$(get_end_line "$@")

skipHeader() {
    echo "$1" | sed '1d'
}

release_notes=$(sed -n "${start_line},${end_line}p" $changelog_file)
release_notes=$(skipHeader "$release_notes")

echo "$release_notes"