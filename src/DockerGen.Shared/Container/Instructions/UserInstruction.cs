using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

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

		[Required]
		[JsonInclude]
		public string User { get; set; }
		[JsonInclude]
		public string Group { get; set; }

		public override string Description => @"The USER instruction sets the user name (or UID) and optionally the user group (or GID) to use when running the image and for any RUN, CMD and ENTRYPOINT instructions that follow it in the Dockerfile.";

		public override string Prefix => "USER";
		public override string DisplayName => "Set which user runs commands";

		protected override void CompileArguments(StringBuilder builder)
		{
			builder.Append(User);
			if(Group != null)
            {
				builder.Append(':');
				builder.Append(Group);
            }
		}

        public static UserInstruction ParseFromString(string line)
        {
			if (!line.StartsWith("USER", StringComparison.InvariantCultureIgnoreCase))
			{
				throw new ParseInstructionException("Not a valid prefix: " + line.Substring(0, line.IndexOf(' ')));
			}

			var userValues = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			if(userValues.Length == 1)
            {
				throw new ParseInstructionException("Missing USER definition");
            }

			var userDefinition = userValues[1].Split(':', StringSplitOptions.RemoveEmptyEntries);
			var instruction = new UserInstruction();
			instruction.User = userDefinition[0];
			if(userDefinition.Length == 2)
            {
				instruction.Group = userDefinition[1];
            }

			return instruction;
		}
    }
}