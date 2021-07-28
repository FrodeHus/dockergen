using System.Text;

namespace DockerGen.Container
{
    public class UserInstruction : Instruction
    {
        public UserInstruction()
        {

        }
        public UserInstruction(string user)
        {
            if (string.IsNullOrEmpty(user))
            {
                throw new System.ArgumentException($"'{nameof(user)}' cannot be null or empty.", nameof(user));
            }
            User = user;
        }

        public string User { get; set; }

        public override string Description => throw new System.NotImplementedException();

        public override string Prefix => "USER";
        public override string DisplayName => "Set user for container processes";

        protected override void CompileArguments(StringBuilder builder)
        {
            builder.Append(User);
        }
    }
}