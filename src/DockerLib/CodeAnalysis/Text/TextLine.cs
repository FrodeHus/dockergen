namespace DockerLib.CodeAnalysis.Text;

public sealed class TextLine
{
    public TextLine(SourceDockerfile source, int start, int length, int lengthIncludingLineBreak)
    {
        Source = source;
        Start = start;
        Length = length;
        LengthIncludingLineBreak = lengthIncludingLineBreak;
    }

    public SourceDockerfile Source { get; }
    public int Start { get; }
    public int Length { get; }
    public int LengthIncludingLineBreak { get; }
}