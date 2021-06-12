using System.Text;

namespace DockerGen.Container
{
    public class EnvironmentInstruction : Instruction
    {
        protected override string Prefix => "ENV";

        protected override void CompileArguments(StringBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}