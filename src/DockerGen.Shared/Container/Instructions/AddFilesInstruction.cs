using System.Text;
using System.Text.Json.Serialization;

namespace DockerGen.Container
{
	public class AddFilesInstruction : Instruction
	{
		public override string Description => "Copies files, directories or remote files URLs from `Source` and adds it to the filesystem of the image at the path `Destination`";
		public override string Prefix => "ADD";
		public override string DisplayName => "Add files from local or remote source";

        [JsonInclude]
		public string LocalOrRemoteSource { get; set; } = "/src";
		[JsonInclude]
		public string Destination { get; set; } = "/app";
		[JsonInclude]
		public string Owner { get; set; }
		[JsonInclude]
		public string Group { get; set; }
		public bool IsOwnershipDefined()
		{
			return !string.IsNullOrEmpty(Owner) || !string.IsNullOrEmpty(Group);
		}
		protected override void CompileArguments(StringBuilder builder)
		{
			if (IsOwnershipDefined())
			{
				builder.Append("--chown=");
				builder.Append(string.Join(":", Owner, Group).Trim(':'));
				builder.Append(' ');
			}

			builder.Append(LocalOrRemoteSource);
			builder.Append(' ');
			builder.Append(Destination);
		}

		public static AddFilesInstruction ParseFromString(string value)
		{
			var values = value.Split(' ');
			if (!values[0].Equals("ADD", StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			var ownerValue = values.SingleOrDefault(v => v.StartsWith("--chown="))?.Split('=');
			var owner = ownerValue?.Length == 2 ? ownerValue[1] : null;
			values = values
				.Where(v => !v.Contains("ADD", StringComparison.OrdinalIgnoreCase) && !v.Contains("chown", StringComparison.OrdinalIgnoreCase))
				.ToArray();

			var instruction = new AddFilesInstruction
			{
				Owner = owner,
				LocalOrRemoteSource = values[0],
				Destination = values[1]
			};
			return instruction;
		}
	}
}
