using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;
public class ImageLiteralSyntax : SyntaxNode
{
    public ImageLiteralSyntax(SourceDockerfile source, LiteralExpressionSyntax registryUserLiteral, LiteralExpressionSyntax repositoryNameLiteral, LiteralExpressionSyntax tagLiteral) : base(source)
    {
        RegistryUserLiteral = registryUserLiteral;
        RepositoryNameLiteral = repositoryNameLiteral;
        TagLiteral = tagLiteral;
    }

    public override SyntaxKind Kind => SyntaxKind.ImageStatement;

    public LiteralExpressionSyntax RegistryUserLiteral { get; }
    public LiteralExpressionSyntax RepositoryNameLiteral { get; }
    public LiteralExpressionSyntax TagLiteral { get; }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return RegistryUserLiteral;
        yield return RepositoryNameLiteral;
        yield return TagLiteral;
    }
}