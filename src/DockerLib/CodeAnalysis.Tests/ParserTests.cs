using DockerLib.CodeAnalysis.Syntax;
using DockerLib.CodeAnalysis.Text;
using Xunit;

namespace CodeAnalysis.Tests;

public class ParserTests
{
    [Theory]
    [InlineData("RUN --secret=mysecret apt update && test", 1)]
    [InlineData("FROM my_user/repo:test\r\nRUN cp . .\\\r\nls -lf", 2)]
    [InlineData("FROM my/repo-name:test AS build\r\nRUN cp . .", 2)]
    [InlineData("FROM my/repo-name:test", 1)]
    public void ParseInstructions(string dockerfile, int expectedInstructions)
    {
        var source = SourceDockerfile.From(dockerfile);
        var parser = new Parser(source);
        var instructions = parser.ParseInstructions();
        Assert.Equal(expectedInstructions, instructions.Length);
    }

    [Theory]
    [InlineData("RUN --secret=mysecret apt update && test", 0)]
    [InlineData("FROM my_user/repo:test\r\nRUN cp . .", 1)]
    [InlineData("FROM my/repo-name:test AS build\r\nRUN cp . .\r\nFROM my/other-image:v1", 2)]
    public void ParseBuildStages(string dockerfile, int expectedNumberOfStages)
    {
        var source = SourceDockerfile.From(dockerfile);
        var parser = new Parser(source);
        var stages = parser.Parse();
        Assert.Equal(expectedNumberOfStages, stages.Length);
    }
}