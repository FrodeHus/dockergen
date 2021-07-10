using System.Text;

namespace DockerGen.Container
{
    public class UserInstruction : Instruction
    {
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

        protected override string Prefix => "USER";

        protected override void CompileArguments(StringBuilder builder)
        {
            builder.Append(User);
        }
    }
}