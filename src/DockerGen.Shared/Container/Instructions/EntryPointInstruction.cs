using System.Text;
using System.Text.Json.Serialization;

namespace DockerGen.Container
{
	public class EntryPointInstruction : Instruction
	{
		public EntryPointInstruction()
		{

		}
		public EntryPointInstruction(string cmd)
		{
			EntryPoint = cmd;
		}

		[JsonInclude]
		public string EntryPoint { get; set; }


		public override string Description => "Ensure the container runs the same executable every time";

		public override string Prefix => "ENTRYPOINT";
		public override string DisplayName => "Set container to run as executable";

		protected override void CompileArguments(StringBuilder builder)
		{

			builder.Append(EntryPoint);
		}

        public static EntryPointInstruction ParseFromString(string line)
        {
			if (!line.StartsWith("ENTRYPOINT", StringComparison.InvariantCultureIgnoreCase))
			{
				throw new ParseInstructionException("Not a valid prefix: " + line.Substring(0, line.IndexOf(' ')));
			}

			var epValue = line[line.IndexOf(' ')..].Trim();
			return new EntryPointInstruction(epValue);
		}
    }
}