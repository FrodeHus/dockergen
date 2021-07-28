using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DockerGen.Container
{
    public static class ContainerService
    {
        private static List<Instruction> ValidInstructions;
        private static List<string> ValidPrefixes;
        public static List<string> GetValidPrefixes()
        {
            EnsureInstructionInformationLoaded();
            return ValidPrefixes;
        }

        public static List<Instruction> GetValidInstructions()
        {
            EnsureInstructionInformationLoaded();
            return ValidInstructions;
        }

        private static void EnsureInstructionInformationLoaded()
        {
            if (ValidInstructions != null)
            {
                return;
            }
            var instructions = Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(Instruction).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract).Select(t => (Instruction)Activator.CreateInstance(t));
            ValidInstructions = instructions.Select(i => i).ToList();
            ValidPrefixes = instructions.Select(i => i.Prefix).ToList();

        }
    }
}
