using System.Diagnostics;
using DockerLib.CodeAnalysis.Syntax;
using DockerLib.CodeAnalysis.Text;

namespace CodeAnalysis.Tests;

public class SyntaxTests
{
    [Theory]
    [InlineData("FROM docker/image as test", SyntaxKind.FromKeyword)]
    [InlineData("COPY . .", SyntaxKind.CopyKeyword)]
    [InlineData("RUN cp . .", SyntaxKind.RunKeyword)]
    [InlineData("WORKDIR /test", SyntaxKind.WorkingDirectoryKeyword)]
    [InlineData("USER nonroot", SyntaxKind.UserKeyword)]
    [InlineData("cp COPY fail", SyntaxKind.BadToken)]
    public void LexInstructionKeywords(string data, SyntaxKind expectedTokenKind)
    {
        var source = SourceDockerfile.From(data);
        var lexer = new Lexer(source);
        var token = lexer.NextToken();
        Assert.Equal(expectedTokenKind, token.Kind);
    }
}