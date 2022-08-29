using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;
public abstract class ExpressionSyntax : SyntaxNode
{
    public ExpressionSyntax(SourceDockerfile source) : base(source)
    {

    }
}