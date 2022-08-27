using System.Diagnostics;
using DockerLib.CodeAnalysis.Syntax;
using DockerLib.CodeAnalysis.Text;

namespace CodeAnalysis.Tests;

public class SyntaxTests
{
    [Theory]
    [InlineData("FROM docker/image as test", SyntaxKind.FromToken)]
    [InlineData("COPY . .", SyntaxKind.CopyToken)]
    [InlineData("RUN cp . .", SyntaxKind.RunToken)]
    [InlineData("WORKDIR /test", SyntaxKind.WorkingDirectoryToken)]
    [InlineData("USER nonroot", SyntaxKind.UserToken)]
    [InlineData("cp COPY fail", SyntaxKind.BadToken)]
    public void LexInstructionKeywords(string data, SyntaxKind expectedTokenKind)
    {
        var source = SourceDockerfile.From(data);
        var lexer = new Lexer(source);
        var token = lexer.NextToken();
        Assert.Equal(expectedTokenKind, token.Kind);
    }
}