using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;

public abstract class InstructionSyntax : SyntaxNode
{
    private protected InstructionSyntax(SourceDockerfile source, SyntaxToken instruction) : base(source)
    {
        Instruction = instruction;
    }

    public SyntaxToken Instruction { get; }
}