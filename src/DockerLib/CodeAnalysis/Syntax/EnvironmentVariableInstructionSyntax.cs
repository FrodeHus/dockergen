using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;

public class EnvironmentVariableInstructionSyntax : InstructionSyntax
{
    public EnvironmentVariableInstructionSyntax(SourceDockerfile source) : base(source)
    {

    }

    public override SyntaxKind Kind => SyntaxKind.EnvironmentVariableInstruction;

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        throw new NotImplementedException();
    }
}