namespace DockerLib.CodeAnalysis.Text;

public record TextLocation(SourceDockerfile Source, TextSpan Span)
{
    public string FileName => Source.Filename;
    public int StartLine => Source.GetLineIndex(Span.Start);
    public int StartCharacter => Span.Start - Source.Lines[StartLine].Start;
    public int EndLine => Source.GetLineIndex(Span.End);
    public int EndCharacter => Span.End - Source.Lines[EndLine].Start;
}
