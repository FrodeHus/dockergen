using System.Text;

namespace DockerGen.Container
{
    public class ArgumentInstruction : Instruction
    {
        public override string Description => throw new System.NotImplementedException();

        public override string Prefix => "ARG";

        protected override void CompileArguments(StringBuilder builder)
        {

        }
    }
}