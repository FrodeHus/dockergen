using DockerGen.Container.Recipes;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Text.RegularExpressions;

namespace DockerGen.Container
{
    public class DynamicRecipe : DynamicObject, IInstruction
    {
        public DynamicRecipe(Recipe recipe)
        {
            _recipe = recipe;
            SetParametersAndDefaultValues();
            InstantiateInstructions();
        }

        public readonly List<IDockerInstruction> Instructions = new();

        private readonly Recipe _recipe;
        private readonly Dictionary<string, object> _parameters = new();
        public string DisplayName => _recipe.Name;
        public string Description => _recipe.Description;
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public List<string> Parameters => _recipe.Parameters.ConvertAll(p => p.Name);

        public event EventHandler<InstructionEventArgs> OnInstructionChanged;

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = GetParameterValue(binder.Name);
            return result != null;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return SetParameterValue(binder.Name, value);
        }
        protected void FireInstructionChanged()
        {
            OnInstructionChanged?.Invoke(this, new InstructionEventArgs(this));
        }

        private object GetParameterValue(string parameterName)
        {
            if (!_parameters.ContainsKey(parameterName))
            {
                return null;
            }
            return _parameters[parameterName];
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
            var builder = new StringBuilder();
            foreach (var instruction in Instructions)
            {
                builder.AppendLine(instruction.Compile());
            }
            return builder.ToString();
        }
        private void InstantiateInstructions()
        {
            if (_recipe.Instructions == null)
            {
                return;
            }

            foreach (var instructionDefinition in _recipe.Instructions)
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
            foreach (var parameter in _recipe.Parameters)
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
