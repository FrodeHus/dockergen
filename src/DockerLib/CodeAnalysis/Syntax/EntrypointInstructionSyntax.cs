using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;

public class EntrypointInstructionSyntax : InstructionSyntax
{
    public EntrypointInstructionSyntax(SourceDockerfile source, SyntaxToken entrypointToken) : base(source)
    {
        EntrypointToken = entrypointToken;
    }
    public override SyntaxKind Kind => SyntaxKind.EntryPointInstruction;

    public SyntaxToken EntrypointToken { get; }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return EntrypointToken;
    }
}