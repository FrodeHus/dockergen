using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;

public class FromInstructionSyntax : InstructionSyntax
{
    public FromInstructionSyntax(SourceDockerfile source, SyntaxToken fromToken, SyntaxToken imageToken, SyntaxToken asToken, SyntaxToken stageNameToken) : base(source)
    {
        FromToken = fromToken;
        ImageToken = imageToken;
        AsToken = asToken;
        StageNameToken = stageNameToken;
    }
    public override SyntaxKind Kind => SyntaxKind.FromInstructionSyntax;

    public SyntaxToken FromToken { get; }
    public SyntaxToken ImageToken { get; }
    public SyntaxToken AsToken { get; }
    public SyntaxToken StageNameToken { get; }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return FromToken;
        yield return ImageToken;
        yield return AsToken;
        yield return StageNameToken;
    }
}