using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis;

public sealed class Diagnostic
{
    private Diagnostic(bool isError, TextLocation location, string message)
    {
        IsError = isError;
        Location = location;
        Message = message;
    }

    public bool IsError { get; }
    public TextLocation Location { get; }
    public string Message { get; }

    public override string ToString() => Message;

    public static Diagnostic Error(TextLocation location, string message)
    {
        return new Diagnostic(true, location, message);
    }

    public static Diagnostic Warning(TextLocation location, string message)
    {
        return new Diagnostic(false, location, message);
    }
}
