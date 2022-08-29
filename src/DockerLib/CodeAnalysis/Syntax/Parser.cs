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

    public ImmutableArray<BuildStageStatement> Parse()
    {
        return ParseBuildStages();
    }
    private ImmutableArray<BuildStageStatement> ParseBuildStages()
    {
        var stages = ImmutableArray.CreateBuilder<BuildStageStatement>();
        while (Current.Kind != SyntaxKind.EndOfFileToken)
        {
            var stage = ParseBuildStage();
            if (stage == null)
            {
                NextToken();
                continue;
            }
            stages.Add(stage);
        }
        return stages.ToImmutable();
    }

    private BuildStageStatement? ParseBuildStage()
    {
        if (Current.Kind != SyntaxKind.FromKeyword && Current.Kind != SyntaxKind.BuildArgumentKeyword)
        {
            Diagnostics.ReportUnexpectedInstruction(Current.Location, Current.Kind, SyntaxKind.FromInstruction, SyntaxKind.BuildArgumentInstruction);
            return null;
        }
        var instructions = ImmutableArray.CreateBuilder<InstructionSyntax>();
        var fromInstruction = (FromInstructionSyntax)ParseFromInstruction();
        while (Current.Kind != SyntaxKind.FromKeyword && Current.Kind != SyntaxKind.EndOfFileToken)
        {
            var start = Current;
            var instruction = ParseInstruction();
            if (instruction == null)
            {
                Diagnostics.ReportNotSupported(Current.Location, Current.Kind);
                break;
            }
            instructions.Add(instruction);
            if (Current == start)
                NextToken();
        }

        return new BuildStageStatement(Source, fromInstruction, instructions.ToImmutable());
    }
    public ImmutableArray<InstructionSyntax> ParseInstructions()
    {
        var instructions = ImmutableArray.CreateBuilder<InstructionSyntax>();
        while (Current.Kind != SyntaxKind.EndOfFileToken)
        {
            var start = Current;
            var instruction = ParseInstruction();
            if (instruction == null)
                continue;

            instructions.Add(instruction);
            if (Current == start)
                NextToken();
        }
        return instructions.ToImmutable();
    }
    private InstructionSyntax? ParseInstruction()
    {

        var instruction = Current.Kind switch
        {
            SyntaxKind.FromKeyword => ParseFromInstruction(),
            SyntaxKind.RunKeyword => ParseRunInstruction(),
            SyntaxKind.ExposeKeyword => ParseExposeInstruction(),
            SyntaxKind.WorkingDirectoryKeyword => ParseWorkDirInstruction(),
            _ => default
        };
        return instruction;
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
        } while (!token.TrailingTrivia.Any(t => t.Kind == SyntaxKind.WhitespaceTriviaToken || t.Kind == SyntaxKind.LineBreakTriviaToken));

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
        var arguments = ImmutableArray.CreateBuilder<ArgumentExpressionSyntax>();
        while (Current.Kind == SyntaxKind.ArgumentSwitchToken)
        {
            var argumentExpression = ParseArgumentExpression();
            arguments.Add(argumentExpression);
        }
        while (Current.Kind != SyntaxKind.EndOfFileToken && (!Current.HasLineBreak() || Current.IsMultiLined()))
        {
            var token = NextToken();
            runParams.Add(token);
        }

        runParams.Add(NextToken());

        var scriptLiteral = new LiteralExpressionSyntax(Source, runParams.ToArray());
        return new RunInstructionSyntax(Source, runToken, arguments.ToImmutable(), scriptLiteral);
    }

    private InstructionSyntax ParseExposeInstruction()
    {
        var exposeToken = MatchToken(SyntaxKind.ExposeKeyword);
        var portToken = MatchToken(SyntaxKind.NumberToken);
        return new ExposeInstructionSyntax(Source, exposeToken, portToken);
    }

    private ArgumentExpressionSyntax ParseArgumentExpression()
    {
        var argumentToken = MatchToken(SyntaxKind.ArgumentSwitchToken);
        var tokens = ImmutableArray.CreateBuilder<SyntaxToken>();
        while (Current.Kind.IsPathCompatible())
        {
            tokens.Add(NextToken());
        }

        var argumentNameLiteral = new LiteralExpressionSyntax(Source, tokens.ToArray());

        var equalToken = MatchToken(SyntaxKind.EqualToken);
        tokens.Clear();
        while (!Current.TrailingTrivia.Any(t => t.Kind == SyntaxKind.WhitespaceTriviaToken))
        {
            tokens.Add(NextToken());
        }
        tokens.Add(NextToken());

        var argumentValueLiteral = new LiteralExpressionSyntax(Source, tokens.ToArray());
        return new ArgumentExpressionSyntax(Source, argumentToken, argumentNameLiteral, equalToken, argumentValueLiteral);
    }
}