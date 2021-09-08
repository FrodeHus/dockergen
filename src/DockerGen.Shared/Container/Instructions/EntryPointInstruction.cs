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
			var values = cmd.Split(' ');
			Executable = values[0];
			Arguments = cmd[(cmd.IndexOf(Executable) + Executable.Length)..];
		}

		[JsonInclude]
		public string Executable { get; set; }

		[JsonInclude]
		public string Arguments { get; set; }

		public override string Description => "Ensure the container runs the same executable every time";

		public override string Prefix => "ENTRYPOINT";
		public override string DisplayName => "Set container to run as executable";

		protected override void CompileArguments(StringBuilder builder)
		{
			builder.Append('[');
			builder.Append('\"').Append(Executable).Append('\"');
			if (Arguments != null)
			{
				var args = Arguments.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				if (args.Length > 0)
				{
					builder.Append(", ");
					for (var i = 0; i < args.Length; i++)
					{
						var arg = args[i];
						builder.Append('"');
						builder.Append(arg);
						builder.Append('"');
						if (i < args.Length - 1)
						{
							builder.Append(", ");
						}
					}
				}
			}
			builder.Append(']');
		}
	}
}