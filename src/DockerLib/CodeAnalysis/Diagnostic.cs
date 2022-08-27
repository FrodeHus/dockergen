namespace DockerLib.CodeAnalysis;

public sealed class Diagnostic
{
    private Diagnostic(bool isError, int position, string message)
    {
        IsError = isError;
        Position = position;
        Message = message;
    }

    public bool IsError { get; }
    public int Position { get; }
    public string Message { get; }

    public override string ToString() => Message;

    public static Diagnostic Error(int position, string message)
    {
        return new Diagnostic(true, position, message);
    }
    public static Diagnostic Warning(int position, string message)
    {
        return new Diagnostic(false, position, message);
    }
}