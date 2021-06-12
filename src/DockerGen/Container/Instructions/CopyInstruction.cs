using System.Text;

namespace DockerGen.Container
{
    public class CopyInstruction : Instruction
    {
        protected override string Prefix => "COPY";

        protected override void CompileArguments(StringBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}