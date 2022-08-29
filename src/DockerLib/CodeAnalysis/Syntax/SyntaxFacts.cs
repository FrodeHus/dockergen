namespace DockerLib.CodeAnalysis.Syntax;

public static class SyntaxFacts
{
    public static bool IsKeyword(this SyntaxKind kind)
    {
        return kind.ToString().EndsWith("Keyword");
    }

    public static bool IsPathCompatible(this SyntaxKind kind)
    {
        return kind switch
        {
            SyntaxKind.StringToken => true,
            SyntaxKind.NumberToken => true,
            SyntaxKind.UnderscoreToken => true,
            SyntaxKind.HyphenToken => true,
            SyntaxKind.ForwardSlash => true,
            SyntaxKind.PeriodToken => true,
            SyntaxKind.BackSlashToken => true,
            _ => false
        };
    }

    public static bool HasLineBreak(this SyntaxToken token)
    {
        return token.TrailingTrivia.Any(t => t.Kind == SyntaxKind.LineBreakTriviaToken);
    }

    public static bool IsMultiLined(this SyntaxToken token)
    {
        return token.TrailingTrivia.Any(t => t.Kind == SyntaxKind.MultiLineTriviaToken);
    }

    public static SyntaxKind GetInstructionKeywordKind(string value)
    {
        return value.ToLowerInvariant() switch
        {
            "from" => SyntaxKind.FromKeyword,
            "run" => SyntaxKind.RunKeyword,
            "copy" => SyntaxKind.CopyKeyword,
            "env" => SyntaxKind.EnvironmentVariableKeyword,
            "arg" => SyntaxKind.BuildArgumentKeyword,
            "expose" => SyntaxKind.ExposeKeyword,
            "workdir" => SyntaxKind.WorkingDirectoryKeyword,
            "user" => SyntaxKind.UserKeyword,
            "volume" => SyntaxKind.VolumeKeyword,
            "healthcheck" => SyntaxKind.HealthCheckKeyword,
            "onbuild" => SyntaxKind.OnBuildKeyword,
            _ => SyntaxKind.BadToken
        };
    }
    public static SyntaxKind GetKeywordKind(string value)
    {
        return value.ToLowerInvariant() switch
        {
            "as" => SyntaxKind.AsKeyword,
            _ => SyntaxKind.BadToken
        };
    }
}