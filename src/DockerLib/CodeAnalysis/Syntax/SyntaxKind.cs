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
    VolumeKeyword,
    AsKeyword,
    StageNameKeyword,
    HealthCheckKeyword,
    OnBuildKeyword,

    //instructions
    FromInstructionSyntax,
    RunInstructionSyntax,
    EntryPointInstructionSyntax,
    CommandInstructionSyntax,
    CopyInstructionSyntax,
    AddInstructionSyntax,
    EnvironmentVariableInstructionSyntax,
    BuildArgumentInstructionSyntax,
    ExposeInstructionSyntax,


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