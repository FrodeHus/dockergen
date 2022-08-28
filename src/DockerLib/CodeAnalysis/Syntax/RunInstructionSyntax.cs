using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax
{
    internal class RunInstructionSyntax : InstructionSyntax
    {


        public RunInstructionSyntax(SourceDockerfile source, SyntaxToken runToken, List<SyntaxToken> runParams) : base(source)
        {
            RunToken = runToken;
            RunParams = runParams;
        }

        public override SyntaxKind Kind => SyntaxKind.RunInstructionSyntax;

        public SyntaxToken RunToken { get; }
        public List<SyntaxToken> RunParams { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return RunToken;
            foreach (var token in RunParams)
                yield return token;
        }
    }
}