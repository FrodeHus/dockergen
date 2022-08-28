using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;
public class WorkingDirectoryInstructionSyntax : InstructionSyntax
{
    public WorkingDirectoryInstructionSyntax(SourceDockerfile source, SyntaxToken workingDirectoryToken, LiteralExpressionSyntax workingDirectoryLiteral) : base(source)
    {
        WorkingDirectoryToken = workingDirectoryToken;
        WorkingDirectoryLiteral = workingDirectoryLiteral;
    }

    public override SyntaxKind Kind => SyntaxKind.WorkingDirectoryInstruction;

    public SyntaxToken WorkingDirectoryToken { get; }
    public LiteralExpressionSyntax WorkingDirectoryLiteral { get; }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return WorkingDirectoryToken;
        yield return WorkingDirectoryLiteral;
    }
}