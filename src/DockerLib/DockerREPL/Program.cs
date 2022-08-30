using System.Text;
using DockerLib.CodeAnalysis.Syntax;
using DockerLib.CodeAnalysis.Text;
using Spectre.Console;

AnsiConsole.WriteLine("Welcome to the Dockerfile REPL (Read Eval Print Loop)!");
AnsiConsole.WriteLine("Type Dockerfile expressions and statements at the prompt and press Enter to evaluate them.");
AnsiConsole.WriteLine("Type 'help to learn more, and type 'exit' to quit.");
AnsiConsole.WriteLine(string.Empty);
var sb = new StringBuilder();
while (true)
{
    var response = AnsiConsole.Prompt<string>(new TextPrompt<string>("> ").PromptStyle("cornflowerblue"));
    if (response == "exit") break;
    switch (response)
    {
        case "print":
            AnsiConsole.WriteLine(sb.ToString());
            break;
        case "build":
            var parser = new Parser(SourceDockerfile.From(sb.ToString()));
            var buildStages = parser.Parse();
            AnsiConsole.WriteLine("");
            var tree = new Tree("Dockerfile");
            foreach (var stage in buildStages)
            {
                RenderInstructionAST(tree, stage);
            }
            AnsiConsole.Write(tree);
            if (parser.Diagnostics.Any())
            {
                AnsiConsole.WriteLine("");
                AnsiConsole.WriteLine("Diagnostics:");
                foreach (var diag in parser.Diagnostics)
                {
                    var level = diag.IsError ? "ERROR" : "WARN";
                    var levelColor = diag.IsError ? "red":"yellow";
                    AnsiConsole.MarkupLine($"\t[{levelColor}]{level}[/]: {diag.Message} - {diag.Location}");
                }
            }
            break;
        default:
            sb.AppendLine(response);
            break;
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
                treeNode.AddNode($"[grey]{trivia.Kind}[/] <{trivia.Span.Length}>");
            }
        }

        var childNode = treeNode.AddNode($"[cyan2]{child.Kind}[/]");
        if (token != null)
        {
            childNode.AddNode($"[yellow]Value: {token.Text}[/]");
            foreach (var trivia in token.TrailingTrivia)
            {
                childNode.AddNode($"[grey]{trivia.Kind}[/] <{trivia.Span.Length}>");
            }
        }

        AddNode(childNode, child);
    }
}

