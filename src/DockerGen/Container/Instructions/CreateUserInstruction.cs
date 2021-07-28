using System.Text;

namespace DockerGen.Container
{
    public class CreateUserInstruction : RunInstruction
    {
        public CreateUserInstruction() : this("nonroot", 9999)
        {

        }
        public CreateUserInstruction(string username = "nonroot", int userId = 9999)
        {
            UserId = userId;
            Username = username;
        }

        public override string DisplayName => "RUN AS";
        public int UserId { get; set; }
        public string Username { get; set; }

        private string Useradd()
        {
            return $"groupadd -r {Username} && useradd --no-log-init -u {UserId} -r -g {Username} {Username}";
        }

        private string Adduser()
        {
            return $"addgroup -S {Username} -g {UserId} && adduser -S {Username} -u {UserId} -G {Username}";
        }

        protected override void CompileArguments(StringBuilder builder)
        {
            builder.Append("if ! command -v useradd &> /dev/null;");
            builder.Append(" then ");
            builder.Append(Adduser());
            builder.Append("; else ");
            builder.Append(Useradd());
            builder.Append("; fi;");
        }
    }
}