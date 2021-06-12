using System;
using Xunit;
using DockerGen.Container;
using FluentAssertions;

namespace DockerGen.Tests
{
    public class InstructionTests
    {
        [Fact]
        public void FROM_Defaults_To_Latest_Tag_If_Not_Specified()
        {
            const string expected = "FROM testimage:latest";
            var instruction = new FromInstruction("testimage");
            var actual = instruction.Compile();
            actual.Should().Be(expected);
        }

        [Fact]
        public void FROM_Adds_Tag_In_Base_Image_If_Included()
        {
            const string expected = "FROM testimage:v1";
            var instruction = new FromInstruction("testimage", "v1");
            var actual = instruction.Compile();
            actual.Should().Be(expected);
        }

        [Fact]
        public void FROM_Adds_StageName_If_Specified()
        {
            const string expected = "FROM testimage:latest AS build";
            var instruction = new FromInstruction("testimage", stageName: "build");
            var actual = instruction.Compile();
            actual.Should().Be(expected);
        }

        [Fact]
        public void FROM_Can_Parse_From_Image_String()
        {
            const string expectedImage = "testimage";
            const string expectedTag = "v1.0";
            FromInstruction instruction = $"from {expectedImage}:{expectedTag} as build";
            instruction.Should().NotBeNull();
            instruction.Image.Should().Be(expectedImage);
            instruction.Tag.Should().Be(expectedTag);
        }
        [Fact]
        public void RUN_Can_Parse_From_String()
        {
            RunInstruction instruction = "run apt update && apt-install -y test && echo 'asdf' > test.txt";
            instruction.Should().NotBeNull();
            instruction.ShellCommand.Should().Be("apt update && apt-install -y test && echo 'asdf' > test.txt");
        }
    }
}
