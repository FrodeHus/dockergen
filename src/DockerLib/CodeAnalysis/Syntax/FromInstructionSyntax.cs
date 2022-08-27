using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;

public class FromInstructionSyntax : InstructionSyntax
{
    public FromInstructionSyntax(SourceDockerfile source, SyntaxToken instruction, SyntaxToken imageToken, SyntaxToken asToken, SyntaxToken stageNameToken) : base(source, instruction)
    {
        ImageToken = imageToken;
        AsToken = asToken;
        StageNameToken = stageNameToken;
    }
    public override SyntaxKind Kind => SyntaxKind.FromInstructionSyntax;

    public SyntaxToken ImageToken { get; }
    public SyntaxToken AsToken { get; }
    public SyntaxToken StageNameToken { get; }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Instruction;
        yield return ImageToken;
        yield return AsToken;
        yield return StageNameToken;
    }
}