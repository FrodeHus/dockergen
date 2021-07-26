﻿using System.Threading.Tasks;
using DockerGen.Components;
using DockerGen.Container;
using Microsoft.AspNetCore.Components;

namespace DockerGen.Pages
{
    public partial class Index : ComponentBase
    {
        private InstructionList _instructions;
        protected void AddInstruction()
        {
            _instructions.AddInstruction(new FromInstruction("busybox"));
            _instructions.AddInstruction(new RunInstruction("echo \"Hello World\""));
            _instructions.AddInstruction(new RunInstruction("apt update && apt -y upgrade && apt -y install ca-certificates curl"));
        }

        private Task InstructionsUpdated()
        {
            StateHasChanged();
            return Task.CompletedTask;
        }
    }
}
