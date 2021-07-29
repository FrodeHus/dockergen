using System.Text;

namespace DockerGen.Container
{
    public class WorkDirInstruction : Instruction
    {
        public WorkDirInstruction()
        {
            Directory = "/app";
        }
        public override string Description => "The WORKDIR instruction sets the working directory for any RUN, CMD, ENTRYPOINT, COPY and ADD instructions that follow it in the Dockerfile. If the WORKDIR doesn’t exist, it will be created even if it’s not used in any subsequent Dockerfile instruction.";

        public override string Prefix => "WORKDIR";
        public override string DisplayName => "Set current directory";
        public string Directory { get; set; }

        protected override void CompileArguments(StringBuilder builder)
        {
            builder.Append(Directory);
        }

        public static implicit operator WorkDirInstruction(string value)
        {
            var values = value.Split(' ');
            if (values.Length != 2 || !values[0].Equals("WORKDIR", System.StringComparison.OrdinalIgnoreCase)) return null;
            return new WorkDirInstruction { Directory = values[1] };
        }
    }
}
