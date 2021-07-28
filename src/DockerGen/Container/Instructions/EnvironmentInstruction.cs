using System.Text;

namespace DockerGen.Container
{
    public class EnvironmentInstruction : Instruction
    {
        public override string Description => throw new System.NotImplementedException();

        public override string Prefix => "ENV";

        protected override void CompileArguments(StringBuilder builder)
        {

        }
    }
}