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
        [Fact]
        public void RUN_Produces_Valid_Instruction()
        {
            const string expected = "RUN apt update";
            var instruction = new RunInstruction("apt update");
            var compiled = instruction.Compile();
            compiled.Should().Be(expected);
        }
        [Fact]
        public void COPY_Produces_Valid_Instruction()
        {
            const string expected = "COPY test.txt .";
            var instruction = new CopyInstruction("test.txt", ".");
            var compiled = instruction.Compile();
            compiled.Should().Be(expected);
        }

        [Fact]
        public void COPY_Can_Set_Ownership_Of_Files()
        {
            const string expected = "COPY --chown=test:test test.txt .";
            var instruction = new CopyInstruction("test.txt", ".");
            instruction.Owner = "test";
            instruction.Group = "test";
            var compiled = instruction.Compile();
            compiled.Should().Be(expected);
        }

        [Fact]
        public void COPY_Can_Set_Source_Location()
        {
            const string expected = "COPY --from=build /app/ .";
            var instruction = new CopyInstruction("/app/", ".");
            instruction.Location = "build";
            var compiled = instruction.Compile();
            compiled.Should().Be(expected);
        }
        [Fact]
        public void USER_Produces_Valid_Instruction()
        {
            const string expected = "USER dummy";
            var instruction = new UserInstruction("dummy");
            var compiled = instruction.Compile();
            compiled.Should().Be(expected);
        }

        [Fact]
        public void Create_User_Produces_Valid_RUN_Instruction()
        {
            const string expected = "RUN if ! command -v useradd &> /dev/null; then addgroup -S dummy -g 9999 && adduser -S dummy -u 9999 -G dummy; else groupadd -r dummy && useradd --no-log-init -u 9999 -r -g dummy dummy; fi;";
            var instruction = new CreateUserInstruction();
            var compiled = instruction.Compile();
            compiled.Should().Be(expected);
        }

        [Fact]
        public void ENTRYPOINT_Produces_Valid_Instruction()
        {
            const string expected = "ENTRYPOINT [\"runmystuff\"]";
            var instruction = new EntryPointInstruction("runmystuff");
            var compiled = instruction.Compile();
            compiled.Should().Be(expected);
        }

        [Fact]
        public void ENTRYPOINT_Supports_Arguments()
        {
            const string expected = "ENTRYPOINT [\"runmystuff\", \"-t\", \"asdf\"]";
            var instruction = new EntryPointInstruction("runmystuff", "-t", "asdf");
            var compiled = instruction.Compile();
            compiled.Should().Be(expected);
        }

        [Fact]
        public void CMD_Produces_Valid_Instruction()
        {
            const string expected = "CMD [\"start\", \"myapp\"]";
            var instruction = new CommandInstruction("start", "myapp");
            var compiled = instruction.Compile();
            compiled.Should().Be(expected);
        }
    }
}
