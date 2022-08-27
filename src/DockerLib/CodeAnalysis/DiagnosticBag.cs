using System.Collections;
using DockerLib.CodeAnalysis.Syntax;
using DockerLib.CodeAnalysis.Text;

namespace DockerLib.CodeAnalysis;

public sealed class DiagnosticBag : IEnumerable<Diagnostic>
{
    private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();


    public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();
    public void AddRange(IEnumerable<Diagnostic> diagnostics) => _diagnostics.AddRange(diagnostics);

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_diagnostics).GetEnumerator();
    }

    private void ReportError(TextLocation location, string message)
    {
        var diagnostic = Diagnostic.Error(location, message);
        _diagnostics.Add(diagnostic);
    }
    private void ReportWarning(TextLocation location, string message)
    {
        var diagnostic = Diagnostic.Warning(location, message);
        _diagnostics.Add(diagnostic);
    }
    public void ReportUnExpectedToken(TextLocation location, SyntaxKind actualKind, SyntaxKind expectedKind)
    {
        var message = $"Unexpected token <{actualKind}>, expected <{expectedKind}>";
        ReportError(location, message);
    }

    internal void ReportInvalidNumber(TextLocation location, string text)
    {
        var message = $"'{text}' is not a valid number";
        ReportError(location, message);
    }
}