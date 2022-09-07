using System.Collections.Immutable;
using System.Diagnostics;
using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;

[DebuggerDisplay("{Kind} - Text: {Text} [Missing: {IsMissing}]")]
public sealed class SyntaxToken : SyntaxNode
{
    public SyntaxToken(
        SourceDockerfile source,
        SyntaxKind kind,
        int position,
        string? text,
        object? value
    ) : base(source)
    {
        Kind = kind;
        Position = position;
        IsMissing = text == null;
        Text = text;
        Value = value;
    }

    public override SyntaxKind Kind { get; }
    public int Position { get; }
    public string? Text { get; }
    public object? Value { get; }
    public ImmutableArray<SyntaxTrivia> LeadingTrivia { get; internal set; } =
        ImmutableArray<SyntaxTrivia>.Empty;
    public ImmutableArray<SyntaxTrivia> TrailingTrivia { get; internal set; } =
        ImmutableArray<SyntaxTrivia>.Empty;
    public bool IsMissing { get; }

    public override TextSpan Span => new(Position, Text?.Length ?? 0);

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        return Array.Empty<SyntaxNode>();
    }
}
