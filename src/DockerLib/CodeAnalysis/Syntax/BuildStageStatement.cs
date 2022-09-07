using System.Collections.Immutable;
using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;

public class BuildStageStatement : SyntaxNode
{
    public BuildStageStatement(
        SourceDockerfile source,
        FromInstructionSyntax fromInstruction,
        ImmutableArray<InstructionSyntax> instructions
    ) : base(source)
    {
        FromInstruction = fromInstruction;
        Instructions = instructions;
    }

    public override SyntaxKind Kind => SyntaxKind.BuildStageStatement;

    public FromInstructionSyntax FromInstruction { get; }
    public ImmutableArray<InstructionSyntax> Instructions { get; }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return FromInstruction;
        foreach (var instruction in Instructions)
            yield return instruction;
    }
}
