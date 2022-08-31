using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;
public class EnvironmentVariableDeclarationStatementSyntax : SyntaxNode
{
    public EnvironmentVariableDeclarationStatementSyntax(SourceDockerfile source, LiteralExpressionSyntax nameLiteral, LiteralExpressionSyntax valueLiteral) : base(source)
    {
        NameLiteral = nameLiteral;
        ValueLiteral = valueLiteral;
    }
    public override SyntaxKind Kind => SyntaxKind.EnvironmentVariableDeclarationStatement;

    public LiteralExpressionSyntax NameLiteral { get; }
    public LiteralExpressionSyntax ValueLiteral { get; }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return NameLiteral;
        yield return ValueLiteral;
    }
}
