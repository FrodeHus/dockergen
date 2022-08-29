using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;
public class ArgumentExpressionSyntax : ExpressionSyntax
{
    public ArgumentExpressionSyntax(SourceDockerfile source, SyntaxToken argumentToken, LiteralExpressionSyntax argumentNameLiteral, SyntaxToken equalToken, LiteralExpressionSyntax argumentValueLiteral) : base(source)
    {
        ArgumentToken = argumentToken;
        ArgumentNameLiteral = argumentNameLiteral;
        EqualToken = equalToken;
        ArgumentValueLiteral = argumentValueLiteral;
    }

    public override SyntaxKind Kind => SyntaxKind.ArgumentExpression;

    public SyntaxToken ArgumentToken { get; }
    public LiteralExpressionSyntax ArgumentNameLiteral { get; }
    public SyntaxToken EqualToken { get; }
    public LiteralExpressionSyntax ArgumentValueLiteral { get; }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return ArgumentToken;
        yield return ArgumentNameLiteral;
        yield return EqualToken;
        yield return ArgumentValueLiteral;
    }
}