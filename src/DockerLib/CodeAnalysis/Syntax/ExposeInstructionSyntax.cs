using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;

public class ExposeInstructionSyntax : InstructionSyntax
{
    public ExposeInstructionSyntax(SourceDockerfile source, SyntaxToken exposeToken, SyntaxToken portToken) : base(source)
    {
        ExposeToken = exposeToken;
        PortToken = portToken;
    }
    public override SyntaxKind Kind => SyntaxKind.ExposeInstruction;

    public SyntaxToken ExposeToken { get; }
    public SyntaxToken PortToken { get; }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return ExposeToken;
        yield return PortToken;
    }
}