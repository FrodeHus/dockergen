using System.Diagnostics;
using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;

public abstract class SyntaxNode
{
    private protected SyntaxNode(SourceDockerfile source)
    {
        Source = source;
    }

    public abstract SyntaxKind Kind { get; }
    public abstract IEnumerable<SyntaxNode> GetChildren();
    public TextLocation Location => new(Source, Span);
    public virtual TextSpan Span
    {
        get
        {
            var first = GetChildren().First().Span;
            var last = GetChildren().Last().Span;
            return TextSpan.FromBounds(first.Start, last.End);
        }
    }

    public SourceDockerfile Source { get; }
}