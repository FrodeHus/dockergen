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
    FromToken,
    RunToken,
    CopyToken,
    AddToken,
    EntrypointToken,
    CommandToken,
    EnvironmentVariableToken,
    BuildArgumentToken,
    ExposeToken,
    UserToken,
    WorkingDirectoryToken,

    //instructions
    FromInstructionSyntax,
    RunInstructionSyntax,
    EntryPointInstructionSyntax,
    CommandInstructionSyntax,
    CopyInstructionSyntax,
    AddInstructionSyntax,
    EnvironmentVariableInstructionSyntax,
    BuildArgumentInstructionSyntax,


    AsToken,
    StageNameToken,
    StringToken
}