using System;

namespace DockerGen.Container
{
    public interface IInstruction
    {
        string DisplayName { get; }
        string Description { get; }
        string Id { get; set; }

        event EventHandler<InstructionEventArgs> OnInstructionChanged;

        string Compile();
    }
}
