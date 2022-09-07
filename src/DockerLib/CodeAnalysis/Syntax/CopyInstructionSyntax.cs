using System.Collections.Immutable;
using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;

public class CopyInstructionSyntax : InstructionSyntax
{
    public CopyInstructionSyntax(
        SourceDockerfile source,
        SyntaxToken copyToken,
        ImmutableArray<ArgumentExpressionSyntax> argumentExpressions,
        LiteralExpressionSyntax sourceLiteral,
        LiteralExpressionSyntax destinationLiteral
    ) : base(source)
    {
        CopyToken = copyToken;
        ArgumentExpressions = argumentExpressions;
        SourceLiteral = sourceLiteral;
        DestinationLiteral = destinationLiteral;
    }

    public override SyntaxKind Kind => SyntaxKind.CopyInstruction;

    public SyntaxToken CopyToken { get; }
    public ImmutableArray<ArgumentExpressionSyntax> ArgumentExpressions { get; }
    public LiteralExpressionSyntax SourceLiteral { get; }
    public LiteralExpressionSyntax DestinationLiteral { get; }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return CopyToken;
        foreach (var argument in ArgumentExpressions)
            yield return argument;
        yield return SourceLiteral;
        yield return DestinationLiteral;
    }
}
