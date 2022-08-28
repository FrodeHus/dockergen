namespace DockerLib.CodeAnalysis.Syntax;

public static class SyntaxFacts
{
    public static bool IsKeyword(this SyntaxKind kind)
    {
        return kind.ToString().EndsWith("Keyword");
    }

    public static SyntaxKind GetKeywordKind(string value)
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
            "as" => SyntaxKind.AsKeyword,
            _ => SyntaxKind.BadToken
        };
    }
}