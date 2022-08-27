using System.Collections.Immutable;
using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;

public class Parser
{
    private readonly ImmutableArray<SyntaxToken> _tokens;
    private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
    private int _position;
    public Parser(SourceDockerfile source)
    {
        SyntaxToken token;
        var tokens = new List<SyntaxToken>();
        var lexer = new Lexer(source);
        do
        {
            token = lexer.NextToken();
            if (token.Kind != SyntaxKind.BadToken)
            {
                tokens.Add(token);
            }
        } while (token.Kind != SyntaxKind.EndOfFileToken);
        _tokens = tokens.ToImmutableArray();
    }

    private SyntaxToken Current => Peek(0);
    private SyntaxToken NextToken()
    {
        var current = Current;
        _position++;
        return current;
    }

    private SyntaxToken MatchToken(SyntaxKind kind)
    {
        if (Current.Kind == kind)
            return NextToken();

        _diagnostics.ReportUnExpectedToken(_position, Current.Kind, kind);
        return new SyntaxToken(kind, Current.Position, null, null);
    }

    private SyntaxToken Peek(int offset)
    {
        var index = _position + offset;
        if (index >= _tokens.Length)
        {
            return _tokens[^1];
        }
        return _tokens[index];
    }

    private void ParseInstructions()
    {
        while (Current.Kind != SyntaxKind.EndOfFileToken)
        {
            var startToken = Current;
            var instruction = ParseInstruction();
            if (Current == startToken)
                NextToken();
        }
    }

    private InstructionSyntax ParseInstruction()
    {
        return default;
    }
}