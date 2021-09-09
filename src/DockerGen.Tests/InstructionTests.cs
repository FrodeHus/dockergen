using DockerGen.Container;
using FluentAssertions;
using Xunit;

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
			FromInstruction instruction = FromInstruction.ParseFromString($"from {expectedImage}:{expectedTag}");
			instruction.Should().NotBeNull();
			instruction.Image.Should().Be(expectedImage);
			instruction.Tag.Should().Be(expectedTag);
		}

		[Fact]
		public void FROM_Can_Parse_From_Image_String_With_StageName()
		{
			const string expectedImage = "testimage";
			const string expectedTag = "v1.0";
			const string expectedStageName = "build";
			FromInstruction instruction = FromInstruction.ParseFromString($"from {expectedImage}:{expectedTag} AS build");
			instruction.Should().NotBeNull();
			instruction.Image.Should().Be(expectedImage);
			instruction.Tag.Should().Be(expectedTag);
			instruction.StageName.Should().Be(expectedStageName);
		}

		[Fact]
		public void RUN_Can_Parse_From_String()
		{
			RunInstruction instruction = RunInstruction.ParseFromString("run apt update && apt-install -y test && echo 'asdf' > test.txt");
			instruction.Should().NotBeNull();
			instruction.ShellCommand.Trim().Should().Be("apt update && apt-install -y test && echo 'asdf' > test.txt");
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
			instruction.Stage = "build";
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
		public void ENTRYPOINT_Produces_Valid_Instruction()
		{
			const string expected = "ENTRYPOINT runmystuff";
			var instruction = new EntryPointInstruction("runmystuff");
			var compiled = instruction.Compile();
			compiled.Should().Be(expected);
		}

		[Fact]
		public void ENTRYPOINT_Supports_Arguments()
		{
			const string expected = "ENTRYPOINT runmystuff -t asdf";
			var instruction = new EntryPointInstruction("runmystuff -t asdf");
			var compiled = instruction.Compile();
			compiled.Should().Be(expected);
		}

		[Fact]
		public void CMD_Produces_Valid_Instruction()
		{
			const string expected = "CMD [\"start\", \"myapp\"]";
			var instruction = new CommandInstruction("start myapp");
			var compiled = instruction.Compile();
			compiled.Should().Be(expected);
		}
		[Fact]
		public void EXPOSE_Produces_Valid_Instructions()
		{
			const string expected = "EXPOSE 80/tcp";
			var instruction = new ExposeInstruction(80, "tcp");
			var compiled = instruction.Compile();
			compiled.Should().Be(expected);
		}

		[Theory]
		[InlineData("expose 80")]
		[InlineData("EXPOSE 80/tcp")]
		[InlineData("expose 80/udp")]
		[InlineData("expose 80/TCP")]
		public void EXPOSE_Can_Parse_From_String(string value)
        {
			var instruction = ExposeInstruction.ParseFromString(value);
			instruction.Should().NotBeNull();
			instruction.Port.Should().Be(80);
			instruction.Protocol.Should().BeOneOf("tcp", "udp");
        }

		[Theory]
		[InlineData("user nonroot")]
		[InlineData("user nonroot:nongroup")]
		[InlineData("user 1000:2000")]
		[InlineData("user 1000")]
		public void USER_Can_Parse_From_String(string value)
        {
			var instruction = UserInstruction.ParseFromString(value);
			instruction.Should().NotBeNull();
			instruction.User.Should().BeOneOf("nonroot", "1000");
			instruction.Group.Should().BeOneOf("nongroup", "2000", null);
        }

		[Theory]
		[InlineData(@"CMD echo ""This is a test."" | wc -")]
		[InlineData(@"cmd [""/usr/bin/wc"",""--help""]")]
		public void CMD_Can_Parse_From_String(string value)
		{
			var instruction = CommandInstruction.ParseFromString(value);
			instruction.Should().NotBeNull();
			instruction.Command.Should().BeOneOf(@"[""/usr/bin/wc"",""--help""]", @"echo ""This is a test."" | wc -");
		}

		[Theory]
		[InlineData("ENTRYPOINT top -c")]
		[InlineData(@"entrypoint [""top"",""-c""]")]
		public void ENTRYPOINT_Can_Parse_From_String(string value)
		{
			var instruction = EntryPointInstruction.ParseFromString(value);
			instruction.Should().NotBeNull();
			instruction.EntryPoint.Should().BeOneOf(@"[""top"",""-c""]", "top -c");
		}

		[Theory]
		[InlineData("ENV foo=bar")]
		[InlineData("ENV foo_env=bar\\ dummy")]
		[InlineData("env foo=bar")]
		[InlineData("ENV foo=\"bar,dummy\"")]
		public void ENV_Can_Parse_From_String(string value)
        {
			var instruction = EnvironmentInstruction.ParseFromString(value);
			instruction.Should().NotBeNull();
			instruction.Variable.Should().BeOneOf("FOO", "FOO_ENV");
			instruction.Value.Should().BeOneOf("bar", "bar\\ dummy", "\"bar,dummy\"");
        }

		[Theory]
		[InlineData("healthcheck curl -f 127.0.0.1 || exit 1")]
		[InlineData("healthcheck --interval=5m --retries=2 curl -f 127.0.0.1 || exit 1")]
		[InlineData("healthcheck --interval=5m --timeout=3s curl -f 127.0.0.1 || exit 1")]
		public void HEALTHCHECK_Can_Parse_From_String(string value)
        {
			var instruction = HealthCheckInstruction.ParseFromString(value);
			instruction.Should().NotBeNull();
			instruction.Interval.Should().BeOneOf(30, 300);
			instruction.Timeout.Should().BeOneOf(30, 3);
			instruction.Retries.Should().BeOneOf(3, 2);
			instruction.Command.Should().Be("curl -f 127.0.0.1 || exit 1");
		}
	}
}
