using System.Text;

namespace DockerGen.Container
{
    public class ArgumentInstruction : Instruction
    {
        protected override string Prefix => "ARG";

        protected override void CompileArguments(StringBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}