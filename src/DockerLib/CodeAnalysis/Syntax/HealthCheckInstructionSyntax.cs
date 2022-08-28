using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;
public class HealthCheckInstructionSyntax : InstructionSyntax
{
    public HealthCheckInstructionSyntax(SourceDockerfile source) : base(source)
    {

    }

    public override SyntaxKind Kind => SyntaxKind.HealthCheckInstruction;

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        throw new NotImplementedException();
    }
}