using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;

public class SyntaxTrivia
{
    public SyntaxTrivia(SourceDockerfile source, SyntaxKind kind, int position, string text)
    {
        Source = source;
        Kind = kind;
        Position = position;
        Text = text;
    }

    public SourceDockerfile Source { get; }
    public SyntaxKind Kind { get; }
    public int Position { get; }
    public string Text { get; }
    public TextSpan Span => new(Position, Text?.Length ?? 0);

    public override string ToString() => Text;
}
