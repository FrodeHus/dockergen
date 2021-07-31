using System;
using System.Collections.Generic;

namespace DockerGen.Container.Recipes
{
    public class Recipe : Configuration
    {
        public List<RecipeParameter> Parameters { get; set; }
        public List<RecipeInstruction> Instructions { get; set; }

    }

    public class RecipeParameter
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public object DefaultValue { get; set; }
        public Type ValueType { get; set; } = typeof(string);
    }

    public class RecipeInstruction
    {
        public string Kind { get; set; }
        public List<RecipeInstructionValue> Values { get; set; }
    }

    public class RecipeInstructionValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
