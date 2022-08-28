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
                SyntaxKind.ExposeKeyword => ParseExposeInstruction(),
                SyntaxKind.WorkingDirectoryKeyword => ParseWorkDirInstruction(),
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
        var imageStatement = ParseImageStatement();
        var asToken = MatchToken(SyntaxKind.AsKeyword, true);
        var stageNameToken = MatchToken(SyntaxKind.StringToken, asToken.IsMissing);
        return new FromInstructionSyntax(Source, fromToken, imageStatement, asToken, stageNameToken);
    }

    private ImageLiteralSyntax ParseImageStatement()
    {
        var expressions = new List<LiteralExpressionSyntax>();
        var tokens = new List<SyntaxToken>();
        SyntaxToken token;
        do
        {
            if (Current.Kind == SyntaxKind.EndOfFileToken) break;
            token = NextToken();
            tokens.Add(token);
        } while (!token.TrailingTrivia.Any(t => t.Kind == SyntaxKind.WhitespaceToken || t.Kind == SyntaxKind.LineBreakToken));

        var literalTokens = new List<SyntaxToken>();
        foreach (var t in tokens)
        {
            if (t.Kind != SyntaxKind.ForwardSlash && t.Kind != SyntaxKind.ColonToken)
            {
                literalTokens.Add(t);
            }
            else
            {
                expressions.Add(new LiteralExpressionSyntax(Source, literalTokens.ToArray()));
                literalTokens.Clear();
            }
        }

        var colonIndex = tokens.FindIndex(t => t.Kind == SyntaxKind.ColonToken);
        LiteralExpressionSyntax tagExpression;
        if (colonIndex == -1)
            tagExpression = new LiteralExpressionSyntax(Source, new SyntaxToken(Source, SyntaxKind.StringToken, Current.Position, null, null));
        else
            tagExpression = new LiteralExpressionSyntax(Source, tokens.GetRange(colonIndex + 1, tokens.Count - colonIndex - 1).ToArray());

        return new ImageLiteralSyntax(Source, expressions[0], expressions[1], tagExpression);
    }

    private InstructionSyntax ParseWorkDirInstruction()
    {
        var workDirToken = MatchToken(SyntaxKind.WorkingDirectoryKeyword);
        var tokens = ImmutableArray.CreateBuilder<SyntaxToken>();
        while (Current.Kind.IsPathCompatible())
        {
            tokens.Add(NextToken());
        }
        var workingDirLiteral = new LiteralExpressionSyntax(Source, tokens.ToArray());
        return new WorkingDirectoryInstructionSyntax(Source, workDirToken, workingDirLiteral);
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

    private InstructionSyntax ParseExposeInstruction()
    {
        var exposeToken = MatchToken(SyntaxKind.ExposeKeyword);
        var portToken = MatchToken(SyntaxKind.NumberToken);
        return new ExposeInstructionSyntax(Source, exposeToken, portToken);
    }
}