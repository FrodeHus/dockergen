using DockerLib.CodeAnalysis.Syntax;
using DockerLib.CodeAnalysis.Text;
using Xunit;

namespace CodeAnalysis.Tests;

public class ParserTests
{
    [Theory]
    [InlineData("RUN apt update && test")]
    [InlineData("RUN apt update && test \\\r\ncp . .")]
    public void ParseInstructions(string dockerfile)
    {
        var source = SourceDockerfile.From(dockerfile);
        var parser = new Parser(source);
        var instructions = parser.Parse();
        Assert.Single(instructions);
    }
}