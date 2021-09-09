using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace DockerGen.Container
{
	public class RunInstruction : Instruction
	{
		private string _shellCommand;

		public RunInstruction()
		{

		}
		public RunInstruction(string shellCommand)
		{
			if (string.IsNullOrEmpty(shellCommand))
			{
				throw new ArgumentException($"'{nameof(shellCommand)}' cannot be null or empty.", nameof(shellCommand));
			}

			ShellCommand = shellCommand;
		}
		public override string Prefix => "RUN";
		public override string DisplayName => "Run a command";
		[Required]
		[JsonInclude]
		public virtual string ShellCommand
		{
			get { return _shellCommand; }
			set
			{
				_shellCommand = Validate(value);
			}
		}

		private string Validate(string value)
		{
			var lines = value.Split('\n', StringSplitOptions.RemoveEmptyEntries);
			var builder = new StringBuilder();
			var firstLine = true;
			foreach (var line in lines.Where(l => !string.IsNullOrEmpty(l)).Select(l => l.TrimEnd('\r', ' ')))
			{
				if (!firstLine)
				{
					builder.Append('\t');
				}

				if (line.EndsWith('\\'))
				{
					builder.AppendLine(line);
				}
				else
				{
					builder.Append(line).Append(' ').AppendLine("\\");
					firstLine = false;
				}
			}
			return builder.ToString().TrimEnd(' ', '\\', '\r', '\n');
		}

		public override string Description => "The RUN instruction will execute any commands in a new layer on top of the current image and commit the results.";

		protected override void CompileArguments(StringBuilder builder)
		{
			builder.Append(ShellCommand);
		}

		public static RunInstruction ParseFromString(string v)
		{
			var match = Regex.Match(v, @"(run\s){0,1}(?<cmd>[\w\.\n\-\'\s\t""\/\=\:\\\;\&\>\!&]+)", RegexOptions.IgnoreCase);
			if (!match.Success)
			{
				return null;
			}

			return new RunInstruction(match.Groups["cmd"].Value);
		}
	}
}