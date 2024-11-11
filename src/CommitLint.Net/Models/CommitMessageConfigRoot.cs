using System.Text.Json.Serialization;

namespace CommitLint.Net.Models;

public sealed class CommitMessageConfigRoot
{
    [JsonPropertyName("config")]
    public CommitMessageConfig? Config { get; set; }
}
