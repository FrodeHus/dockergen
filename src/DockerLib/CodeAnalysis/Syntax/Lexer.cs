using System.Text;
using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis.Syntax;

public class Lexer
{
    private int _position;
    private readonly SourceDockerfile _source;

    public Lexer(SourceDockerfile source)
    {
        _source = source;
    }

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
            case '"':
                kind = SyntaxKind.StringToken;
                value = ReadString();
                break;
            case ' ':
                kind = SyntaxKind.WhitespaceToken;
                value = " ";
                break;
            default:
                if (_position == 0 && char.IsLetter(Current))
                {
                    (kind, value) = ReadIdentifierOrKeyword();
                }
                else
                {
                    kind = SyntaxKind.StringToken;
                    value = ReadString();
                }
                break;
        }
        return new SyntaxToken(kind, start, value?.ToString(), value);
    }

    private string ReadString()
    {
        _position++;
        var sb = new StringBuilder();
        var done = false;
        while (!done)
        {
            switch (Current)
            {
                case '\\':
                    if (LookAhead == '\n' || LookAhead == '\r')
                    {
                        _position += 2;
                        break;
                    }
                    else
                    {
                        sb.Append(Current);
                        _position++;
                    }
                    break;
                case '\0':
                case '\r':
                case '\n':
                    done = true;
                    break;
                case '"':
                    if (LookAhead == '"')
                    {
                        sb.Append(Current);
                        _position += 2;
                    }
                    else
                    {
                        _position++;
                        done = true;
                    }
                    break;
                default:
                    sb.Append(Current);
                    _position++;
                    break;
            }
        }
        return sb.ToString();
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
}