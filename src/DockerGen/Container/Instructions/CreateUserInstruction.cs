using System.Text;

namespace DockerGen.Container
{
    public class CreateUserInstruction : RunInstruction
    {
        public CreateUserInstruction(string username = "dummy", int userId = 9999)
        {
            UserId = userId;
            Username = username;
        }
        public int UserId { get; set; }
        public string Username { get; set; }
        protected override void CompileArguments(StringBuilder builder)
        {
            builder.Append("addgroup -S ");
            builder.Append(Username);
            builder.Append(" && ");
            builder.Append("adduser -S ");
            builder.Append(Username);
            builder.Append(" -u ");
            builder.Append(UserId);
            builder.Append(" -G ");
            builder.Append(Username);
        }
    }
}