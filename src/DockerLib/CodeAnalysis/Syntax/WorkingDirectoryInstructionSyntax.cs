using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;
public class WorkingDirectoryInstructionSyntax : InstructionSyntax
{
    public WorkingDirectoryInstructionSyntax(SourceDockerfile source) : base(source)
    {

    }

    public override SyntaxKind Kind => SyntaxKind.WorkingDirectoryInstruction;

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        throw new NotImplementedException();
    }
}