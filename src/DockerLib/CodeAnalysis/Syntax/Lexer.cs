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
        var start = _position;
        var kind = SyntaxKind.BadToken;
        object? value = null;
        switch (Current)
        {
            case '\0':
                kind = SyntaxKind.EndOfFileToken;
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
            case '\r':
            case '\n':
                kind = ReadLineBreak();
                break;
            case ' ':
            case '\t':
                kind = ReadWhiteSpace();
                break;
            case '&':
                kind = SyntaxKind.AmpersandToken;
                _position++;
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
            default:
                if (char.IsWhiteSpace(Current))
                {
                    kind = ReadWhiteSpace();
                }
                else if (_source.GetPositionInLine(_position) == 0 && char.IsLetter(Current))
                {
                    (kind, value) = ReadIdentifierOrKeyword();
                }
                else if (char.IsLetterOrDigit(Current))
                {
                    kind = SyntaxKind.StringToken;
                    value = ReadString();
                }
                else
                {
                    kind = SyntaxKind.BadToken;
                    value = Current;
                    _position++;
                }

                break;
        }
        return new SyntaxToken(_source, kind, start, value?.ToString(), value);
    }

    private (SyntaxKind, int) ReadNumber()
    {
        var start = _position;
        while (char.IsDigit(Current))
            _position++;

        var length = _position - start;
        var text = _source.ToString(start, _position);
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

    private (SyntaxKind, string) ReadIdentifierOrKeyword()
    {
        var start = _position;
        while (char.IsLetter(Current))
            _position++;
        var length = _position - start;
        var text = _source.ToString(start, length);
        var kind = text.ToLowerInvariant() switch
        {
            "from" => SyntaxKind.FromToken,
            "as" => SyntaxKind.AsToken,
            "run" => SyntaxKind.RunToken,
            "copy" => SyntaxKind.CopyToken,
            "env" => SyntaxKind.EnvironmentVariableToken,
            "arg" => SyntaxKind.BuildArgumentToken,
            "expose" => SyntaxKind.ExposeToken,
            "workdir" => SyntaxKind.WorkingDirectoryToken,
            "user" => SyntaxKind.UserToken,
            _ => SyntaxKind.BadToken
        };
        return (kind, text);
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