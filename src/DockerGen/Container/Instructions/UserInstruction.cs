using System.Text;

namespace DockerGen.Container
{
    public class UserInstruction : Instruction
    {
        private string user;

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

        public string User
        {
            get { return user; }
            set
            {
                user = value;
                FireInstructionChanged();
            }
        }

        public override string Description => throw new System.NotImplementedException();

        public override string Prefix => "USER";
        public override string DisplayName => "Set which user runs commands";

        protected override void CompileArguments(StringBuilder builder)
        {
            builder.Append(User);
        }
    }
}