﻿using System.Text;
using DockerLib.CodeAnalysis.Syntax;
using DockerLib.CodeAnalysis.Text;
using PrettyPrompt;
using PrettyPrompt.Consoles;
using PrettyPrompt.Highlighting;
using Spectre.Console;

var console = new SystemConsole();
var prompt = new Prompt(configuration: new PromptConfiguration(
    prompt: new FormattedString(">>> ", new FormatSpan(0, 1, AnsiColor.Red), new FormatSpan(1, 1, AnsiColor.Yellow), new FormatSpan(2, 1, AnsiColor.Green))
));
console.WriteLine("Welcome to the Dockerfile REPL (Read Eval Print Loop)!");
console.WriteLine("Type Dockerfile expressions and statements at the prompt and press Enter to evaluate them.");
console.WriteLine("Type 'help to learn more, and type 'exit' to quit.");
console.WriteLine(string.Empty);
var sb = new StringBuilder();
while (true)
{
    var response = await prompt.ReadLineAsync();
    if (response.IsSuccess)
    {
        if (response.Text == "exit") break;
        switch (response.Text)
        {
            case "print":
                console.WriteLine(sb.ToString());
                break;
            case "build":
                var parser = new Parser(SourceDockerfile.From(sb.ToString()));
                var buildStages = parser.Parse();
                console.WriteLine("");
                var tree = new Tree("Dockerfile");
                foreach (var stage in buildStages)
                {
                    RenderInstructionAST(tree, stage);
                }
                AnsiConsole.Write(tree);
                if (parser.Diagnostics.Any())
                {
                    console.WriteLine("");
                    console.WriteLine("Diagnostics:");
                    foreach (var diag in parser.Diagnostics)
                    {
                        var level = diag.IsError ? "ERROR" : "WARN";
                        console.WriteLine($"\t{level}: {diag.Message} - {diag.Location}");
                    }
                }
                break;
            default:
                sb.AppendLine(response.Text);
                break;
        }
    }
}

static void RenderInstructionAST(Tree tree, SyntaxNode node)
{
    var parentNode = tree.AddNode(node.Kind.ToString());
    AddNode(parentNode, node);
}

static void AddNode(TreeNode treeNode, SyntaxNode node)
{
    foreach (var child in node.GetChildren())
    {
        var token = child as SyntaxToken;
        if (token != null)
        {
            foreach (var trivia in token.LeadingTrivia)
            {
                treeNode.AddNode($"[grey]Lead: {trivia.Kind}[/] <{trivia.Span.Length}>");
            }
        }

        var childNode = treeNode.AddNode($"[cyan2]{child.Kind}[/]");
        if (token != null)
        {
            childNode.AddNode($"[yellow]Value: {token.Text}[/]");
            foreach (var trivia in token.TrailingTrivia)
            {
                childNode.AddNode($"[grey]Trail: {trivia.Kind}[/] <{trivia.Span.Length}>");
            }
        }

        AddNode(childNode, child);
    }
}

