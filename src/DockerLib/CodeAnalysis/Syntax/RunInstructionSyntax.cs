using System.Collections.Immutable;
using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax
{
    internal class RunInstructionSyntax : InstructionSyntax
    {


        public RunInstructionSyntax(SourceDockerfile source, SyntaxToken runToken, ImmutableArray<ArgumentExpressionSyntax> argumentExpressions, List<SyntaxToken> runParams) : base(source)
        {
            RunToken = runToken;
            ArgumentExpressions = argumentExpressions;
            RunParams = runParams;
        }

        public override SyntaxKind Kind => SyntaxKind.RunInstruction;

        public SyntaxToken RunToken { get; }
        public ImmutableArray<ArgumentExpressionSyntax> ArgumentExpressions { get; }
        public List<SyntaxToken> RunParams { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return RunToken;
            foreach (var argument in ArgumentExpressions)
            {
                yield return argument;
            }

            foreach (var token in RunParams)
                yield return token;
        }
    }
}