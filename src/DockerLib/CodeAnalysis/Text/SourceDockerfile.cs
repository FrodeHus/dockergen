using System.Collections.Immutable;

namespace DockerLib.CodeAnalysis.Text;

public sealed class SourceDockerfile
{
    private readonly string _text;
    private SourceDockerfile(string text, string filename)
    {
        _text = text;
        Filename = filename;
        Lines = ParseLines(this, text);
    }

    public string Filename { get; }
    public static SourceDockerfile From(string text, string fileName = "")
    {
        return new SourceDockerfile(text, fileName);
    }

    public ImmutableArray<TextLine> Lines { get; }
    public char this[int index] => _text[index];
    public int Length => _text.Length;
    public override string ToString() => _text;
    public string ToString(int start, int length) => _text.Substring(start, length);
    public int GetLineIndex(int position)
    {
        var lower = 0;
        var upper = Lines.Length - 1;
        while (lower <= upper)
        {
            var index = lower + (upper - lower) / 2;
            var start = Lines[index].Start;
            if (position == start)
            {
                return index;
            }
            if (start > position)
            {
                upper = index - 1;
            }
            else
            {
                lower = index + 1;
            }

        }
        return lower - 1;
    }
    private static ImmutableArray<TextLine> ParseLines(SourceDockerfile source, string text)
    {
        var result = ImmutableArray.CreateBuilder<TextLine>();
        var position = 0;
        var lineStart = 0;

        while (position < text.Length)
        {
            var lineBreakWidth = GetLineBreakWidth(text, position);
            if (lineBreakWidth == 0)
            {
                position++;
            }
            else
            {
                AddLine(result, source, position, lineStart, lineBreakWidth);
                lineStart = position;
            }
        }

        if (position >= lineStart)
        {
            AddLine(result, source, position, lineStart, 0);
        }
        return result.ToImmutable();
    }

    private static void AddLine(ImmutableArray<TextLine>.Builder result, SourceDockerfile source, int position, int lineStart, int lineBreakWidth)
    {
        var lineLength = position - lineStart;
        var lengthIncludingLineBreak = lineLength + lineBreakWidth;
        var line = new TextLine(source, lineStart, lineLength, lengthIncludingLineBreak);
        result.Add(line);
    }

    private static int GetLineBreakWidth(string text, int position)
    {
        var c = text[position];
        var l = position + 1 >= text.Length ? '\0' : text[position + 1];
        if (c == '\r' && l == '\n')
            return 2;
        if (c == '\r' || c == '\n')
            return 1;
        return 0;
    }
}