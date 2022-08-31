using System.Collections.Immutable;
using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;

public class EnvironmentVariableInstructionSyntax : InstructionSyntax
{
    public EnvironmentVariableInstructionSyntax(SourceDockerfile source, SyntaxToken envToken, ImmutableArray<EnvironmentVariableDeclarationStatementSyntax> declarationStatements) : base(source)
    {
        EnvToken = envToken;
        DeclarationStatements = declarationStatements;
    }

    public override SyntaxKind Kind => SyntaxKind.EnvironmentVariableInstruction;

    public SyntaxToken EnvToken { get; }
    public ImmutableArray<EnvironmentVariableDeclarationStatementSyntax> DeclarationStatements { get; }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return EnvToken;
        foreach (var declaration in DeclarationStatements)
            yield return declaration;
    }
}