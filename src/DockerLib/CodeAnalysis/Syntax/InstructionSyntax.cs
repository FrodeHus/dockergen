using System.Diagnostics;
using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;

[DebuggerDisplay("{Instruction}")]
public abstract class InstructionSyntax : SyntaxNode
{
    private protected InstructionSyntax(SourceDockerfile source, SyntaxToken instruction) : base(source)
    {
        Instruction = instruction;
    }

    public SyntaxToken Instruction { get; }
}