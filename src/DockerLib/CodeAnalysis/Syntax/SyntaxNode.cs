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
    private void PrettyPrint(StringBuilder writer, SyntaxNode node, string indent = "", bool isLast = true)
    {
        var tokenMarker = isLast ? "└──" : "├──";
        writer.Append(indent);
        writer.Append(tokenMarker);
        writer.Append(node.Kind);
        if (node is SyntaxToken token && !string.IsNullOrEmpty(token.Text))
        {
            writer.Append(' ')
                .Append(token.Text);
        }
        indent += isLast ? "   " : "│  ";
        var lastChild = GetChildren().LastOrDefault();
        writer.AppendLine();
        foreach (var child in node.GetChildren())
        {
            PrettyPrint(writer, child, indent, isLast: child == lastChild);
        }
    }
    public override string ToString()
    {
        var sb = new StringBuilder();
        PrettyPrint(sb, this);
        return sb.ToString();
    }
}