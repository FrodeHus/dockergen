namespace DockerLib.CodeAnalysis.Syntax;

public class FromInstructionSyntax : InstructionSyntax
{
    private readonly SyntaxToken _args;

    public FromInstructionSyntax(SyntaxToken from, SyntaxToken args)
    {
        From = from;
        _args = args;
    }
    public override SyntaxKind Kind => SyntaxKind.FromInstructionSyntax;

    public SyntaxToken From { get; }
}