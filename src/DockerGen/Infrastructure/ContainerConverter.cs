using DockerGen.Container;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DockerGen.Infrastructure
{
    public class ContainerConverter : JsonConverter<ContainerImage>
    {
        public override bool CanConvert(Type typeToConvert) => typeof(ContainerImage).IsAssignableFrom(typeToConvert);

        public override ContainerImage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected object");
            }
            var container = new ContainerImage();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return container;
                }
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("Expected propertyName");
                }

                var propertyName = reader.GetString();
                reader.Read();
                switch (propertyName)
                {
                    case "Stages":
                        var stages = ReadStages(ref reader);
                        container.Stages = stages;
                        break;
                }
            }
            throw new JsonException();
        }

        private List<BuildStage> ReadStages(ref Utf8JsonReader reader)
        {
            var stages = new List<BuildStage>();
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return stages;
                }
                var stage = ReadStage(ref reader);
                stages.Add(stage);
            }
            throw new JsonException();
        }

        private BuildStage ReadStage(ref Utf8JsonReader reader)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }
            var stage = new BuildStage(new FromInstruction("scratch"), "");
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return stage;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case "StageName":
                            var stageName = reader.GetString();
                            stage.StageName = stageName;
                            break;
                        case "BaseImage":
                            var (image, tag) = ReadBaseImage(ref reader);
                            stage.BaseImage.Image = image;
                            stage.BaseImage.Tag = tag;
                            break;
                        case "Instructions":
                            var instructions = ReadInstructions(ref reader);
                            stage.Instructions = instructions;
                            break;
                        default:
                            break;
                    }
                }
            }
            throw new JsonException();
        }

        private List<IInstruction> ReadInstructions(ref Utf8JsonReader reader)
        {
            var instructions = new List<IInstruction>();
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return instructions;
                }
                var instruction = ReadInstruction(ref reader);
                if (instruction != null)
                {
                    instructions.Add(instruction);
                }
            }
            throw new JsonException();
        }

        private IInstruction ReadInstruction(ref Utf8JsonReader reader)
        {

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var kind = "";
            var parameters = new Dictionary<string, string>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    if (kind == "Recipe")
                    {
                        return InstantiateRecipe(parameters);
                    }
                    return InstantiateInstruction(kind, parameters);
                }
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }
                var propertyName = reader.GetString();
                reader.Read();
                switch (propertyName)
                {
                    case "Kind":
                        kind = reader.GetString();
                        break;
                    case "Parameters":
                        parameters = ReadParameters(ref reader);
                        break;
                }

            }
            throw new JsonException();
        }

        private Dictionary<string, string> ReadParameters(ref Utf8JsonReader reader)
        {
            var parameters = new Dictionary<string, string>();
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return parameters;
                }
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                var parameter = reader.GetString();
                reader.Read();
                var value = reader.GetString();
                parameters.Add(parameter, value);
            }
            throw new JsonException();
        }
        private IInstruction InstantiateRecipe(Dictionary<string, string> parameters)
        {
            var recipe = ContainerService.Recipes.SingleOrDefault(r => r.Name == parameters["Name"]);
            if (recipe == null)
            {
                return null;
            }
            foreach (var parameter in recipe.Parameters)
            {
                var value = parameters[parameter.Name];
                parameter.DefaultValue = value;
            }

            return new DynamicRecipe(recipe);
        }

        private IInstruction InstantiateInstruction(string kind, Dictionary<string, string> parameters)
        {
            var instance = Activator.CreateInstance(Type.GetType("DockerGen.Container." + kind));
            foreach (var kvp in parameters)
            {
                SetValue(instance, kvp.Key, kvp.Value);
            }
            return instance as IInstruction;
        }

        private void SetValue(object instance, string property, object value)
        {
            instance.GetType().GetProperty(property).SetValue(instance, value, null);
        }

        private (string, string) ReadBaseImage(ref Utf8JsonReader reader)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }
            string image = "";
            string tag = "";
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return (image, tag);
                }
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case "Image":
                            image = reader.GetString();
                            break;
                        case "Tag":
                            tag = reader.GetString();
                            break;
                    }
                }
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, ContainerImage value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Stages");
            writer.WriteStartArray();
            foreach (var stage in value.Stages)
            {
                WriteStage(writer, stage);
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        private void WriteStage(Utf8JsonWriter writer, BuildStage stage)
        {
            writer.WriteStartObject();
            writer.WriteString("StageName", stage.StageName);

            writer.WriteStartObject("BaseImage");
            writer.WriteString("Image", stage.BaseImage.Image);
            writer.WriteString("Tag", stage.BaseImage.Tag);
            writer.WriteEndObject();

            writer.WriteStartArray("Instructions");
            foreach (var instruction in stage.Instructions)
            {
                if (instruction is DynamicRecipe recipe)
                {
                    WriteRecipe(writer, recipe);
                }
                else
                {
                    WriteInstruction(writer, instruction);
                }
            }
            writer.WriteEndArray();
            writer.WriteEndObject();

        }

        private void WriteRecipe(Utf8JsonWriter writer, DynamicRecipe dynamicRecipe)
        {
            var recipe = dynamicRecipe.Recipe;
            writer.WriteStartObject();
            writer.WriteString("Kind", "Recipe");
            writer.WriteStartObject("Parameters");

            writer.WriteString("Name", recipe.Name);
            foreach (var parameter in dynamicRecipe.Parameters)
            {
                writer.WriteString(parameter, dynamicRecipe[parameter]);
            }
            writer.WriteEndObject();
            writer.WriteEndObject();
        }

        private void WriteInstruction(Utf8JsonWriter writer, IInstruction instruction)
        {
            writer.WriteStartObject();
            writer.WriteString("Kind", instruction.GetType().Name);
            writer.WriteStartObject("Parameters");
            foreach (var property in GetIncludedProperties(instruction))
            {
                writer.WriteString(property.Name, property.GetValue(instruction)?.ToString());
            }
            writer.WriteEndObject();
            writer.WriteEndObject();
        }

        private IEnumerable<PropertyInfo> GetIncludedProperties(object instruction)
        {
            var t = instruction.GetType();
            var includedProperties = new List<PropertyInfo>();
            foreach (var property in t.GetProperties().Where(p => p.IsDefined(typeof(JsonIncludeAttribute))))
            {
                includedProperties.Add(property);
            }
            return includedProperties;
        }
    }
}
