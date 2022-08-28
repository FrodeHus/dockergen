using DockerLib.CodeAnalysis.Syntax;
using DockerLib.CodeAnalysis.Text;
using Xunit;

namespace CodeAnalysis.Tests;

public class ParserTests
{
    [Theory]
    [InlineData("RUN --secret=mysecret apt update && test", 1)]
    [InlineData("RUN apt update && test \\\r\ncp . .", 1)]
    [InlineData("FROM my_user/repo:test\r\nRUN cp . .", 2)]
    [InlineData("FROM my/repo-name:test AS build\r\nRUN cp . .", 2)]
    [InlineData("FROM my/repo-name:test", 1)]
    public void ParseInstructions(string dockerfile, int expectedInstructions)
    {
        var source = SourceDockerfile.From(dockerfile);
        var parser = new Parser(source);
        var instructions = parser.Parse();
        Assert.Equal(expectedInstructions, instructions.Length);
    }
}