using System.Collections;
using DockerLib.CodeAnalysis.Syntax;

namespace DockerLib.CodeAnalysis;

internal sealed class DiagnosticBag : IEnumerable<Diagnostic>
{
    private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();


    public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();
    public void AddRange(IEnumerable<Diagnostic> diagnostics) => _diagnostics.AddRange(diagnostics);

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_diagnostics).GetEnumerator();
    }

    private void ReportError(int position, string message)
    {
        var diagnostic = Diagnostic.Error(position, message);
        _diagnostics.Add(diagnostic);
    }
    private void ReportWarning(int position, string message)
    {
        var diagnostic = Diagnostic.Warning(position, message);
        _diagnostics.Add(diagnostic);
    }
    public void ReportUnExpectedToken(int position, SyntaxKind actualKind, SyntaxKind expectedKind)
    {
        var message = $"Unexpected token <{actualKind}>, expected <{expectedKind}>";
        ReportError(position, message);
    }
}