using System.Text;
using DockerLib.CodeAnalysis.Syntax;
using DockerLib.CodeAnalysis.Text;
using Spectre.Console;

AnsiConsole.Write(new FigletText("Dockerfile AST Decompiler").Color(Color.Cyan1));
AnsiConsole.WriteLine(
    "Type Dockerfile instructions at the prompt and press Enter to evaluate them."
);
AnsiConsole.WriteLine("Type 'help to learn more, and type 'exit' to quit.");
AnsiConsole.WriteLine(string.Empty);
var sb = new StringBuilder();
while (true)
{
    var response = AnsiConsole.Prompt<string>(
        new TextPrompt<string>("> ").PromptStyle("cornflowerblue")
    );
    if (response == "exit")
        break;
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
                var table = new Table();
                table.AddColumns("Level", "Message", "Location");
                foreach (var diag in parser.Diagnostics)
                {
                    var level = diag.IsError ? "ERROR" : "WARN";
                    var levelColor = diag.IsError ? "red" : "yellow";
                    var nearValue = diag.Location.Source.ToString(
                        diag.Location.Span.Start - 4,
                        diag.Location.Span.Length + 4
                    );
                    table.AddRow(
                        $"[{levelColor}]{level}[/]",
                        $"{diag.Message} - near '..{nearValue}'",
                        $"[grey]<{diag.Location.Span}>[/]"
                    );
                }
                AnsiConsole.Write(table);
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
