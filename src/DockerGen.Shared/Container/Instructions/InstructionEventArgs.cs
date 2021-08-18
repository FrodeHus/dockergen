namespace DockerGen.Container
{
    public class InstructionEventArgs
    {
        public InstructionEventArgs(IInstruction instruction)
        {
            Instruction = instruction;
        }

        public IInstruction Instruction { get; set; }
    }
}