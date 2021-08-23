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
		protected override void CompileArguments(StringBuilder builder)
		{
			builder.Append(LocalOrRemoteSource);
			builder.Append(' ');
			builder.Append(Destination);
		}
	}
}
