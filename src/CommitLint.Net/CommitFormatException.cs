namespace CommitLint.Net;

internal class CommitFormatException : Exception
{
    public CommitFormatException(string message)
        : base(message) { }
}
