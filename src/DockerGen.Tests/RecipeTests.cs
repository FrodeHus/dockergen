using DockerGen.Container;
using DockerGen.Container.Recipes;
using FluentAssertions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace DockerGen.Tests
{
    public class RecipeTests
    {
        [Fact]
        public async Task It_Can_Load_Recipes_From_YAML()
        {
            var value = await File.ReadAllTextAsync(Path.Combine("..", "..", "..", "..", "..", "recipes", "RunAsUser.yml"));
            var recipe = RecipeLoader.Parse(value);
            recipe.Should().NotBeNull();
            recipe.Kind.Should().Be("Recipe");
            recipe.Name.Should().Be("Run as user");
            recipe.Parameters.Should().HaveCount(2);
            recipe.Instructions.Should().HaveCount(2);
        }

        [Fact]
        public void DynamicRecipe_Can_Retrieve_Parameter_Value()
        {
            var recipe = new Recipe
            {
                Parameters = new List<RecipeParameter>
                {
                    new RecipeParameter
                    {
                        Name="Username",
                        DefaultValue="nonroot"
                    }
                }
            };
            dynamic dynamicRecipe = new DynamicRecipe(recipe);
            var defaultValue = dynamicRecipe.Username;
            (defaultValue as string)?.Should().Be("nonroot");
        }

        [Fact]
        public void DynamicRecipe_Can_Set_Parameter_Value()
        {
            var recipe = new Recipe
            {
                Parameters = new List<RecipeParameter>
                {
                    new RecipeParameter
                    {
                        Name="Username",
                        DefaultValue="nonroot"
                    }
                }
            };
            dynamic dynamicRecipe = new DynamicRecipe(recipe);
            var defaultValue = dynamicRecipe.Username;
            (defaultValue as string)?.Should().Be("nonroot");
            dynamicRecipe.Username = "root";
            var value = dynamicRecipe.Username;
            (value as string)?.Should().Be("root");
        }

        [Fact]
        public void DynamicRecipe_Can_Instantiate_Instructions()
        {
            var recipe = new Recipe
            {
                Parameters = new List<RecipeParameter>
                {
                    new RecipeParameter
                    {
                        Name="Username",
                        DefaultValue="nonroot"
                    }
                },
                Instructions = new()
                {
                    new RecipeInstruction
                    {
                        Kind = "RunInstruction",
                        Values = new List<RecipeInstructionValue>
                        {
                            new RecipeInstructionValue
                            {
                                Name="ShellCommand",
                                Value="echo Hello ${Username}!"
                            }
                        }
                    }
                }
            };

            var dynamicRecipe = new DynamicRecipe(recipe);
            dynamicRecipe.Instructions.Should().HaveCount(1);
        }

        [Fact]
        public void DynamicRecipe_Can_Set_InstructionValues_With_Parameters()
        {
            const string expected = "echo Hello nonroot!";
            var recipe = new Recipe
            {
                Parameters = new List<RecipeParameter>
                {
                    new RecipeParameter
                    {
                        Name="Username",
                        DefaultValue="nonroot"
                    }
                },
                Instructions = new()
                {
                    new RecipeInstruction
                    {
                        Kind = "RunInstruction",
                        Values = new List<RecipeInstructionValue>
                        {
                            new RecipeInstructionValue
                            {
                                Name="ShellCommand",
                                Value="echo Hello ${Username}!"
                            }
                        }
                    }
                }
            };

            var dynamicRecipe = new DynamicRecipe(recipe);
            dynamicRecipe.Instructions[0].As<RunInstruction>().ShellCommand.Should().Be(expected);
        }
    }
}
