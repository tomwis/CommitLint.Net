namespace CommitLint.Net.Models;

public sealed class ScopesConfig
{
    public bool Enabled { get; set; } = true;
    public List<string>? Global { get; set; }
    public Dictionary<string, List<string>>? PerType { get; set; }
}
