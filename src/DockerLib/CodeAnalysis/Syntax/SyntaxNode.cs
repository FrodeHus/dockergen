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
        var token = node as SyntaxToken;
        if (token != null)
        {
            foreach (var trivia in token.LeadingTrivia)
            {
                writer.Append(indent);
                writer.Append("├──");
                writer.AppendLine($"L: {trivia.Kind}");
            }
        }

        var hasTrailingTrivia = token != null && token.TrailingTrivia.Any();
        var tokenMarker = !hasTrailingTrivia && isLast ? "└──" : "├──";

        writer.Append(indent);
        writer.Append(tokenMarker);
        writer.Append(node.Kind);
        if (token != null && !string.IsNullOrEmpty(token.Text))
        {
            writer.Append(' ')
                .Append(token.Text);
        }
        indent += isLast ? "   " : "│  ";
        var lastChild = GetChildren().LastOrDefault();
        writer.AppendLine();
        if (token != null)
        {
            foreach (var trivia in token.TrailingTrivia)
            {
                var isLastTrailingTrivia = trivia == token.TrailingTrivia.Last();
                var triviaMarker = isLast && isLastTrailingTrivia ? "└──" : "├──";
                writer.Append(indent);
                writer.Append(triviaMarker);
                writer.AppendLine($"T: {trivia.Kind}");
            }
        }
        foreach (var child in node.GetChildren())
        {
            PrettyPrint(writer, child, indent, isLast: child == lastChild);
        }
    }
    public string ToString(bool prettyPrint = false)
    {
        if (prettyPrint)
        {
            var sb = new StringBuilder();
            PrettyPrint(sb, this);
            return sb.ToString();
        }
        else
        {
            return ToString();
        }
    }
}