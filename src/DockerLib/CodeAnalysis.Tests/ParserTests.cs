using DockerLib.CodeAnalysis.Syntax;
using DockerLib.CodeAnalysis.Text;
using Xunit;

namespace CodeAnalysis.Tests;

public class ParserTests
{
    [Theory]
    [InlineData("RUN --secret=mysecret apt update && test", SyntaxKind.RunInstruction)]
    [InlineData("RUN cp . .\\\r\nls -lf", SyntaxKind.RunInstruction)]
    [InlineData("FROM my/repo-name:test AS build\r\nRUN cp . .", SyntaxKind.FromInstruction)]
    [InlineData("WORKDIR /app", SyntaxKind.WorkingDirectoryInstruction)]
    [InlineData("EXPOSE 8080", SyntaxKind.ExposeInstruction)]
    [InlineData("COPY . .", SyntaxKind.CopyInstruction)]
    public void ParseInstructions(string dockerfile, SyntaxKind expectedInstructionKind)
    {
        var source = SourceDockerfile.From(dockerfile);
        var parser = new Parser(source);
        var instructions = parser.ParseInstructions();
        Assert.Equal(expectedInstructionKind, instructions[0].Kind);
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