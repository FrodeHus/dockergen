using System.Collections.Immutable;
using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;

public class Parser
{
    private readonly ImmutableArray<SyntaxToken> _tokens;
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
        Source = source;
    }

    public DiagnosticBag Diagnostics { get; } = new DiagnosticBag();

    private SyntaxToken Current => Peek(0);

    public SourceDockerfile Source { get; }

    private SyntaxToken NextToken()
    {
        var current = Current;
        _position++;
        return current;
    }

    private SyntaxToken MatchToken(SyntaxKind kind, bool isOptional = false)
    {
        if (Current.Kind == kind)
            return NextToken();

        Diagnostics.ReportUnexpectedToken(Current.Location, Current.Kind, kind, isOptional);
        return new SyntaxToken(Source, kind, Current.Position, null, null);
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

    public ImmutableArray<InstructionSyntax> Parse()
    {
        return ParseInstructions();
    }
    private ImmutableArray<InstructionSyntax> ParseInstructions()
    {
        var instructions = new List<InstructionSyntax>();
        while (Current.Kind != SyntaxKind.EndOfFileToken)
        {
            var startToken = Current;

            var instruction = Current.Kind switch
            {
                SyntaxKind.FromKeyword => ParseFromInstruction(),
                SyntaxKind.RunKeyword => ParseRunInstruction(),
                _ => default
            };
            if (instruction is not null)
            {
                instructions.Add(instruction);
            }
            if (Current == startToken)
                NextToken();
        }
        return instructions.ToImmutableArray();
    }

    private InstructionSyntax ParseFromInstruction()
    {
        var fromToken = MatchToken(SyntaxKind.FromKeyword);
        var imageToken = MatchToken(SyntaxKind.StringToken);
        var asToken = MatchToken(SyntaxKind.AsKeyword, isOptional: true);
        var stageNameToken = MatchToken(SyntaxKind.StringToken, isOptional: asToken.IsMissing);
        return new FromInstructionSyntax(Source, fromToken, imageToken, asToken, stageNameToken);
    }

    private InstructionSyntax ParseRunInstruction()
    {
        var runToken = MatchToken(SyntaxKind.RunKeyword);
        var runParams = new List<SyntaxToken>();
        var isMultiline = false;
        while ((Current.Kind != SyntaxKind.LineBreakToken || isMultiline) && Current.Kind != SyntaxKind.EndOfFileToken)
        {
            if (Current.Kind == SyntaxKind.MultiLineToken)
                isMultiline = true;

            if (Current.Kind == SyntaxKind.LineBreakToken && isMultiline)
                isMultiline = false;

            var token = NextToken();
            runParams.Add(token);
        }
        return new RunInstructionSyntax(Source, runToken, runParams);
    }
}