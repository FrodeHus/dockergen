using System.Text;
using System.Text.Json.Serialization;

namespace DockerGen.Container
{
    public class AddFilesInstruction : Instruction
    {
        public override string Description => "Copes files, directories or remote files URLs from `Source` and adds ito th the filesystem of the image at the path `Destination`";
        public override string Prefix => "ADD";

        [JsonInclude]
        public string LocalOrRemoteSource { get; set; }
        [JsonInclude]
        public string Destination { get; set; }
        protected override void CompileArguments(StringBuilder builder)
        {
            builder.Append(LocalOrRemoteSource);
            builder.Append(' ');
            builder.Append(Destination);
        }
    }
}
