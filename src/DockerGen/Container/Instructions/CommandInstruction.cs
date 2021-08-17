using DockerGen.Components.Instructions;
using System.Text;
using System.Text.Json.Serialization;

namespace DockerGen.Container
{
	public class CommandInstruction : Instruction
	{
		public CommandInstruction()
		{

		}
		public CommandInstruction(string command)
		{
			Command = command;
		}

		[JsonInclude]
		public string Command { get; set; }
		public override Type UIType => typeof(Command);

		public override string Description => "The main purpose of a CMD is to provide defaults for an executing container. These defaults can include an executable, or they can omit the executable, in which case you must specify an ENTRYPOINT instruction as well.";

		public override string Prefix => "CMD";
		public override string DisplayName => "Set default command to run if nothing is already specified";

		protected override void CompileArguments(StringBuilder builder)
		{
			builder.Append('[');
			var commands = Command?.Split(' ') ?? null;
			if (commands != null)
			{
				for (var i = 0; i < commands.Length; i++)
				{
					var arg = commands[i];
					builder.Append('"');
					builder.Append(arg);
					builder.Append('"');
					if (i < commands.Length - 1)
					{
						builder.Append(", ");
					}
				}
			}
			builder.Append(']');
		}
	}
}