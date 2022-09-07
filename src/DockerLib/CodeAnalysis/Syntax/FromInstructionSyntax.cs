using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;

public class FromInstructionSyntax : InstructionSyntax
{
    public FromInstructionSyntax(
        SourceDockerfile source,
        SyntaxToken fromToken,
        ImageLiteralSyntax imageStatement,
        SyntaxToken asToken,
        SyntaxToken stageNameToken
    ) : base(source)
    {
        FromToken = fromToken;
        ImageStatement = imageStatement;
        AsToken = asToken;
        StageNameToken = stageNameToken;
    }

    public override SyntaxKind Kind => SyntaxKind.FromInstruction;

    public SyntaxToken FromToken { get; }
    public ImageLiteralSyntax ImageStatement { get; }
    public SyntaxToken AsToken { get; }
    public SyntaxToken StageNameToken { get; }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return FromToken;
        yield return ImageStatement;
        yield return AsToken;
        yield return StageNameToken;
    }
}
