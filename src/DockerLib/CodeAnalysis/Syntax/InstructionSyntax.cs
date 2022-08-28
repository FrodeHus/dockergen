using System.Diagnostics;
using System.Text;
using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;

[DebuggerDisplay("{Instruction}")]
public abstract class InstructionSyntax : SyntaxNode
{
    private protected InstructionSyntax(SourceDockerfile source) : base(source)
    {
    }

}