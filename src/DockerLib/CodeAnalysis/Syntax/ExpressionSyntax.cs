using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;

public abstract class ExpressionSyntax : SyntaxNode
{
    protected ExpressionSyntax(SourceDockerfile source) : base(source) { }
}
