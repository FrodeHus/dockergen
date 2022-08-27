using System.Diagnostics;
using System.Text;
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
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendFormat("[{0}]", Kind.ToString());
        sb.AppendLine();
        var lastChild = GetChildren().LastOrDefault();
        var indent = "  ";
        foreach (var node in GetChildren())
        {
            var token = node as SyntaxToken;
            if (token?.IsMissing ?? false) continue;
            var marker = node == lastChild ? "└──" : "├──";
            sb.Append(indent);
            sb.Append(marker);
            sb.AppendFormat("[{0}]", node.Kind);
            if (token != null)
            {
                sb.AppendFormat("  {0}", token.Text);
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}