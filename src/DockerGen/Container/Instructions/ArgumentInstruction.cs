using System.Text;

namespace DockerGen.Container
{
    public class ArgumentInstruction : Instruction
    {
        public override string Description => throw new System.NotImplementedException();

        public override string Prefix => "ARG";
        public override string DisplayName => "Define build argument";

        protected override void CompileArguments(StringBuilder builder)
        {

        }
    }
}