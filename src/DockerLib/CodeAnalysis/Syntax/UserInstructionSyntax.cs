using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;
public class UserInstructionSyntax : InstructionSyntax
{
    public UserInstructionSyntax(SourceDockerfile source) : base(source)
    {

    }

    public override SyntaxKind Kind => SyntaxKind.UserInstruction;

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        throw new NotImplementedException();
    }
}