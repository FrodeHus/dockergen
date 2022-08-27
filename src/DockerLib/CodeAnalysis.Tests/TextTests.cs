using DockerLib.CodeAnalysis.Text;

namespace CodeAnalysis.Tests;

public class TextTests
{
    [Theory]
    [InlineData("hello world\r\nthis is a test\r\nI think", 15, 1)]
    [InlineData("hello world\r\nthis is a test\r\nI think", 5, 0)]
    public void FindLineNumberFromPosition(string source, int position, int expectedLineIndex)
    {
        var sourceFile = SourceDockerfile.From(source);
        var actualIndex = sourceFile.GetLineIndex(position);
        Assert.Equal(expectedLineIndex, actualIndex);
    }

    [Theory]
    [InlineData("hello world\r\nthis is a test\r\nI think", 13, 0)]
    [InlineData("hello world\r\nthis is a test\r\nI think", 2, 2)]
    public void FindPositionInLine(string source, int position, int expectedLinePosition)
    {
        var sourceFile = SourceDockerfile.From(source);
        var actualLinePosition = sourceFile.GetPositionInLine(position);
        Assert.Equal(expectedLinePosition, actualLinePosition);
    }
}