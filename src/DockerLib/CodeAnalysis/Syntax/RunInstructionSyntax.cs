using System.Collections.Immutable;
using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax
{
    internal class RunInstructionSyntax : InstructionSyntax
    {
        public RunInstructionSyntax(
            SourceDockerfile source,
            SyntaxToken runToken,
            ImmutableArray<ArgumentExpressionSyntax> argumentExpressions,
            LiteralExpressionSyntax scriptLiteral
        ) : base(source)
        {
            RunToken = runToken;
            ArgumentExpressions = argumentExpressions;
            ScriptLiteral = scriptLiteral;
        }

        public override SyntaxKind Kind => SyntaxKind.RunInstruction;

        public SyntaxToken RunToken { get; }
        public ImmutableArray<ArgumentExpressionSyntax> ArgumentExpressions { get; }
        public LiteralExpressionSyntax ScriptLiteral { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return RunToken;
            foreach (var argument in ArgumentExpressions)
            {
                yield return argument;
            }

            yield return ScriptLiteral;
        }
    }
}
