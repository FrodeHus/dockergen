namespace DockerLib.CodeAnalysis.Syntax;
public enum SyntaxKind
{
    WhitespaceToken,
    BadToken,
    EndOfFileToken,

    EqualToken,
    OpenParenthesisToken,
    CloseParenthesisToken,

    //keywords
    FromKeyword,
    RunKeyword,
    CopyKeyword,
    AddKeyword,
    EntrypointKeyword,
    CommandKeyword,
    EnvironmentVariableKeyword,
    BuildArgumentKeyword,
    ExposeKeyword,
    UserKeyword,
    WorkingDirectoryKeyword,
    AsKeyword,
    StageNameKeyword,

    //instructions
    FromInstructionSyntax,
    RunInstructionSyntax,
    EntryPointInstructionSyntax,
    CommandInstructionSyntax,
    CopyInstructionSyntax,
    AddInstructionSyntax,
    EnvironmentVariableInstructionSyntax,
    BuildArgumentInstructionSyntax,
    VolumeKeyword,
    HealthCheckKeyword,
    OnBuildKeyword,


    StringToken,
    LineBreakToken,
    CommaToken,
    NumberToken,
    EndOfLineToken,
    BackSlashToken,
    MultiLineToken,
    AmpersandToken,
    PeriodToken,
    QuoteToken,
    UnderscoreToken,
    DashToken,
    ColonToken,
    SemicolonToken,
    CommentToken,
    ForwardSlash
}