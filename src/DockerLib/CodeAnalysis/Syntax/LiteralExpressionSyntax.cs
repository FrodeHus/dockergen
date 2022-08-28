using System.Diagnostics;
using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;
[DebuggerDisplay("{Kind} - {Value}")]
public class LiteralExpressionSyntax : SyntaxNode
{
    public LiteralExpressionSyntax(SourceDockerfile source, params SyntaxToken[] literalTokens) : base(source)
    {
        LiteralTokens = literalTokens;
        Value = string.Empty;
        foreach (var token in literalTokens)
        {
            Value += token.Text;
        }
    }

    public string Value { get; }
    public SyntaxToken[] LiteralTokens { get; }

    public override SyntaxKind Kind => SyntaxKind.LiteralExpression;

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        foreach (var token in LiteralTokens)
            yield return token;
    }
}