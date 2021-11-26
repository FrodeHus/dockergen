using DockerGen.Container.Recipes;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DockerGen.Container
{
    public class DynamicRecipe : DynamicObject, IInstruction
    {
        public DynamicRecipe(Recipe recipe)
        {
            Recipe = recipe;
            SetParametersAndDefaultValues();
            InstantiateInstructions();
        }

        public readonly List<IDockerInstruction> Instructions = new();

        public readonly Recipe Recipe;
        private readonly Dictionary<string, object> _parameters = new();
        public string DisplayName => Recipe.Name;
        public string Description => Recipe.Description;
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public List<string> Parameters => Recipe.Parameters.ConvertAll(p => p.Name);


        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = GetParameterValue(binder.Name);
            return result != null;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return SetParameterValue(binder.Name, value);
        }

        public string this[string parameterName]
        {
            get
            {
                return _parameters[parameterName]?.ToString();
            }
            set
            {
                _parameters[parameterName] = value;
            }
        }
        private object GetParameterValue(string parameterName)
        {
            if (!_parameters.ContainsKey(parameterName))
            {
                return null;
            }
            return _parameters[parameterName];
        }

        private Type GetValueType(string parameterName)
        {
            var valueType = Recipe.Parameters.Single(p => p.Name == parameterName).ValueType;
            Type type = null;
            switch (valueType.ToLowerInvariant())
            {
                case "string":
                    type = typeof(string);
                    break;
                case "integer":
                    type = typeof(Int32);
                    break;
                default: break;
            }
            return type;
        }


        private bool SetParameterValue(string parameterName, object value)
        {
            if (!_parameters.ContainsKey(parameterName))
            {
                return false;
            }
            _parameters[parameterName] = value;
            return true;
        }
        public string Compile()
        {
            InstantiateInstructions();
            var builder = new StringBuilder();
            foreach (var instruction in Instructions)
            {
                builder.AppendLine(instruction.Compile());
            }
            return builder.ToString();
        }
        private void InstantiateInstructions()
        {
            if (Recipe.Instructions == null)
            {
                return;
            }
            Instructions.Clear();
            foreach (var instructionDefinition in Recipe.Instructions)
            {
                if (Activator.CreateInstance(Type.GetType("DockerGen.Container." + instructionDefinition.Kind)) is not IDockerInstruction instance)
                {
                    throw new ArgumentException("The specified instruction kind is not a Docker instruction or does not exist", nameof(instructionDefinition.Kind));
                }
                SetValues(instructionDefinition, instance);
                Instructions.Add(instance);
            }
        }

        private void SetValues(RecipeInstruction instruction, IDockerInstruction instance)
        {
            foreach (var value in instruction.Values)
            {
                var property = instance.GetType().GetProperty(value.Name);
                var parsedValue = ParseInstructionValue(value.Value);
                property.SetValue(instance, parsedValue);
            }
        }

        private void SetParametersAndDefaultValues()
        {
            foreach (var parameter in Recipe.Parameters)
            {
                _parameters.Add(parameter.Name, parameter.DefaultValue);
            }
        }
        private string ParseInstructionValue(string value)
        {
            return Regex.Replace(value, @"\${(?<parameter>[\w]+)}", (m) =>
            {
                var parameterName = m.Groups["parameter"].Value;
                return GetParameterValue(parameterName)?.ToString();
            });
        }
    }
}
