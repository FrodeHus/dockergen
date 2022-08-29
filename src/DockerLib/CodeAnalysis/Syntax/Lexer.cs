using System.Collections.Immutable;
using System.Text;
using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;

public sealed class Lexer
{
    private int _position;
    private readonly SourceDockerfile _source;

    public Lexer(SourceDockerfile source)
    {
        _source = source;
    }
    public DiagnosticBag Diagnostics { get; } = new();
    private char Current => Peek(0);
    private char LookAhead => Peek(1);
    private char Peek(int offset)
    {
        var index = _position + offset;
        if (index >= _source.Length)
            return '\0';
        return _source[index];
    }

    public SyntaxToken NextToken()
    {
        var leadingTrivia = ReadTrivia(true);
        var token = ReadToken();
        var trailingTrivia = ReadTrivia(false);
        token.LeadingTrivia = leadingTrivia;
        token.TrailingTrivia = trailingTrivia;
        return token;
    }
    private SyntaxToken ReadToken()
    {
        var start = _position;
        var kind = SyntaxKind.BadToken;
        object? value = null;
        switch (Current)
        {
            case '\0':
                kind = SyntaxKind.EndOfFileToken;
                break;
            case '+':
                if (LookAhead == '+')
                {
                    kind = SyntaxKind.PlusPlusToken;
                    _position += 2;
                }
                else
                {
                    kind = SyntaxKind.PlusToken;
                    _position++;
                }
                break;
            case '*':
                kind = SyntaxKind.StarToken;
                _position++;
                break;
            case '(':
                kind = SyntaxKind.OpenParenthesisToken;
                _position++;
                break;
            case ')':
                kind = SyntaxKind.CloseParenthesisToken;
                _position++;
                break;
            case '=':
                kind = SyntaxKind.EqualToken;
                _position++;
                break;
            case ',':
                kind = SyntaxKind.CommaToken;
                _position++;
                break;
            case '.':
                kind = SyntaxKind.PeriodToken;
                _position++;
                break;
            case '\\':
                if (LookAhead == ' ' || LookAhead == '\r' || LookAhead == '\n')
                {
                    kind = SyntaxKind.MultiLineToken;
                }
                else
                {
                    kind = SyntaxKind.BackSlashToken;
                }
                _position++;
                break;
            case '/':
                kind = SyntaxKind.ForwardSlash;
                _position++;
                break;
            case '&':
                if (LookAhead == '&')
                {
                    _position += 2;
                    kind = SyntaxKind.AmpersandAmpersandToken;
                }
                else
                {
                    kind = SyntaxKind.AmpersandToken;
                    _position++;
                }
                break;
            case '<':
                if (LookAhead == '=')
                {
                    kind = SyntaxKind.LessOrEqualsToken;
                    _position += 2;
                }
                else
                {
                    kind = SyntaxKind.LessToken;
                    _position++;
                }
                break;
            case '>':
                if (LookAhead == '=')
                {
                    kind = SyntaxKind.GreaterOrEqualsToken;
                    _position += 2;
                }
                else
                {
                    kind = SyntaxKind.GreaterToken;
                    _position++;
                }
                break;
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
                (kind, value) = ReadNumber();
                break;
            case '"':
                kind = SyntaxKind.QuoteToken;
                _position++;
                break;
            case '`':
                kind = SyntaxKind.BackTickToken;
                _position++;
                break;
            case '\'':
                kind = SyntaxKind.SinqleQuoteToken;
                _position++;
                break;
            case '$':
                kind = SyntaxKind.DollarSignToken;
                _position++;
                break;
            case '%':
                kind = SyntaxKind.PercentageToken;
                _position++;
                break;
            case '@':
                kind = SyntaxKind.AtSignToken;
                _position++;
                break;
            case '#':
                kind = SyntaxKind.HashToken;
                _position++;
                break;
            case '|':
                kind = SyntaxKind.PipeToken;
                _position++;
                break;
            case '?':
                kind = SyntaxKind.QuestionMarkToken;
                _position++;
                break;
            case '[':
                kind = SyntaxKind.OpenSquareBracketToken;
                _position++;
                break;
            case ']':
                kind = SyntaxKind.CloseSquareBracketToken;
                _position++;
                break;
            case '{':
                kind = SyntaxKind.OpenBraceToken;
                _position++;
                break;
            case '}':
                kind = SyntaxKind.CloseBraceToken;
                _position++;
                break;
            case '~':
                kind = SyntaxKind.TildeToken;
                _position++;
                break;
            case '!':
                kind = SyntaxKind.BangToken;
                _position++;
                break;
            case '_':
                kind = SyntaxKind.UnderscoreToken;
                _position++;
                break;
            case '-':
                if (LookAhead == '-')
                {
                    kind = SyntaxKind.ArgumentSwitchToken;
                    _position += 2;
                }
                else
                {
                    kind = SyntaxKind.HyphenToken;
                    _position++;
                }
                break;
            case ':':
                kind = SyntaxKind.ColonToken;
                _position++;
                break;
            case ';':
                kind = SyntaxKind.SemicolonToken;
                _position++;
                break;
            default:

                if (_source.GetPositionInLine(_position) == 0 && char.IsLetter(Current))
                {
                    value = ReadString();
                    kind = SyntaxFacts.GetKeywordKind(value?.ToString() ?? string.Empty);
                }
                else if (char.IsLetterOrDigit(Current))
                {
                    value = ReadString();
                    kind = SyntaxFacts.GetKeywordKind(value?.ToString() ?? string.Empty);
                    if (kind == SyntaxKind.BadToken)
                        kind = SyntaxKind.StringToken;
                }
                else
                {
                    var span = new TextSpan(_position, 1);
                    var location = new TextLocation(_source, span);
                    Diagnostics.ReportBadCharacter(location, Current);
                    _position++;
                }

                break;
        }
        return new SyntaxToken(_source, kind, start, _source.ToString(start, _position - start), value);
    }

    private object? ReadArgumentName()
    {
        _position += 2;
        var start = _position;
        while (char.IsLetter(Current))
            _position++;
        return _source.ToString(start, _position - start);
    }

    private ImmutableArray<SyntaxTrivia> ReadTrivia(bool isLeading)
    {
        var triviaBuilder = ImmutableArray.CreateBuilder<SyntaxTrivia>();
        var done = false;
        while (!done)
        {
            var start = _position;
            var kind = SyntaxKind.BadToken;
            object? value = null;
            switch (Current)
            {
                case '\0':
                    done = true;
                    break;
                case '/':
                    if (LookAhead == '/')
                    {
                        kind = SyntaxKind.CommentToken;
                        value = ReadSingleLineComment();
                    }
                    else
                    {
                        done = true;
                    }
                    break;
                case '\r':
                case '\n':
                    if (!isLeading)
                        done = true;
                    kind = ReadLineBreak();
                    break;
                case ' ':
                case '\t':
                    kind = ReadWhiteSpace();
                    break;
                default:
                    if (char.IsWhiteSpace(Current))
                        ReadWhiteSpace();
                    else
                        done = true;
                    break;
            }

            var length = _position - start;
            if (length > 0)
            {
                var text = _source.ToString(start, length);
                var trivia = new SyntaxTrivia(_source, kind, start, text);
                triviaBuilder.Add(trivia);
            }
        }
        return triviaBuilder.ToImmutableArray();
    }
    private string ReadSingleLineComment()
    {
        _position += 2;
        var start = _position;
        var done = false;
        while (!done)
        {
            switch (Current)
            {
                case '\0':
                case '\r':
                case '\n':
                    done = true;
                    break;
                default:
                    _position++;
                    break;
            }
        }
        var length = _position - start;
        var value = _source.ToString(start, length);
        return value;
    }

    private (SyntaxKind, int) ReadNumber()
    {
        var start = _position;
        while (char.IsDigit(Current))
            _position++;

        var length = _position - start;
        var text = _source.ToString(start, length);
        if (!int.TryParse(text, out var value))
        {
            var span = new TextSpan(start, length);
            var location = new TextLocation(_source, span);
            Diagnostics.ReportInvalidNumber(location, text);
        }
        return (SyntaxKind.NumberToken, value);
    }
    private SyntaxKind ReadLineBreak()
    {
        if (Current == '\r' && LookAhead == '\n')
        {
            _position += 2;
        }
        else
        {
            _position++;
        }
        return SyntaxKind.LineBreakToken;
    }

    private SyntaxKind ReadWhiteSpace()
    {
        var done = false;
        while (!done)
        {
            switch (Current)
            {
                case '\0':
                case '\r':
                case '\n':
                    done = true;
                    break;
                default:
                    if (!char.IsWhiteSpace(Current))
                        done = true;
                    else _position++;
                    break;
            }
        }
        return SyntaxKind.WhitespaceToken;
    }

    private string ReadString()
    {
        var start = _position;
        while (char.IsLetterOrDigit(Current))
            _position++;

        var length = _position - start;
        var text = _source.ToString(start, length);
        return text;
    }
}