using DockerLib.CodeAnalysis.Syntax;
using DockerLib.CodeAnalysis.Text;
using PrettyPrompt;
using PrettyPrompt.Consoles;
using PrettyPrompt.Highlighting;

var console = new SystemConsole();
var prompt = new Prompt(configuration: new PromptConfiguration(
    prompt: new FormattedString(">>> ", new FormatSpan(0, 1, AnsiColor.Red), new FormatSpan(1, 1, AnsiColor.Yellow), new FormatSpan(2, 1, AnsiColor.Green))
));
console.WriteLine("Welcome to the Dockerfile REPL (Read Eval Print Loop)!");
console.WriteLine("Type Dockerfile expressions and statements at the prompt and press Enter to evaluate them.");
console.WriteLine($"Type 'help to learn more, and type 'exit' to quit.");
console.WriteLine(string.Empty);
while (true)
{
    var response = await prompt.ReadLineAsync();
    if (response.IsSuccess)
    {
        if (response.Text == "exit") break;
        var parser = new Parser(SourceDockerfile.From(response.Text));
        var instructions = parser.Parse();
        var instruction = instructions.FirstOrDefault();
        console.WriteLine("");
        console.WriteLine(instruction?.ToString());
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
    }
}
