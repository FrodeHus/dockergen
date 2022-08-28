using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;
public class CopyInstructionSyntax : InstructionSyntax
{
    public CopyInstructionSyntax(SourceDockerfile source) : base(source)
    {

    }

    public override SyntaxKind Kind => SyntaxKind.CopyInstruction;

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        throw new NotImplementedException();
    }
}