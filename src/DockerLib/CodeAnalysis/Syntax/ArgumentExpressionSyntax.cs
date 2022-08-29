using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;
public class ArgumentExpressionSyntax : SyntaxNode
{
    public ArgumentExpressionSyntax(SourceDockerfile source, SyntaxToken argumentToken, LiteralExpressionSyntax argumentNameLiteral, LiteralExpressionSyntax argumentValueLiteral) : base(source)
    {
        ArgumentToken = argumentToken;
        ArgumentNameLiteral = argumentNameLiteral;
        ArgumentValueLiteral = argumentValueLiteral;
    }

    public override SyntaxKind Kind => SyntaxKind.ArgumentExpression;

    public SyntaxToken ArgumentToken { get; }
    public LiteralExpressionSyntax ArgumentNameLiteral { get; }
    public LiteralExpressionSyntax ArgumentValueLiteral { get; }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return ArgumentToken;
        yield return ArgumentNameLiteral;
        yield return ArgumentValueLiteral;
    }
}