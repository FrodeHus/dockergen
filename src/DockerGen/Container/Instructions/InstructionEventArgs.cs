namespace DockerGen.Container
{
    public class InstructionEventArgs
    {
        public InstructionEventArgs(Instruction instruction)
        {
            Instruction = instruction;
        }

        public Instruction Instruction { get; set; }
    }
}