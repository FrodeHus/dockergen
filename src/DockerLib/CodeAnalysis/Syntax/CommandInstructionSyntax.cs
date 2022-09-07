using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;

public class CommandInstructionSyntax : InstructionSyntax
{
    public CommandInstructionSyntax(SourceDockerfile source) : base(source) { }

    public override SyntaxKind Kind => SyntaxKind.CommandInstruction;

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        throw new NotImplementedException();
    }
}
