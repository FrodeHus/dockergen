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
        public string Directory { get; set; }

        protected override void CompileArguments(StringBuilder builder)
        {
            builder.Append(Directory);
        }
    }
}
