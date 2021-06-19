using System;
using Xunit;
using DockerGen.Container;
using FluentAssertions;

namespace DockerGen.Tests
{
    public class ImageTests
    {
        [Fact]
        public void Generate_Basic_Image()
        {
            const string expected = @"FROM busybox
RUN apt install dummy
ENTRYPOINT [""dummy""]";
            var image = new ContainerImage();
            var defaultStage = new BuildStage();
            defaultStage.BaseImage = "busybox";
            defaultStage.Instructions.Add(new RunInstruction("apt install dummy"));
            defaultStage.Instructions.Add(new EntryPointInstruction("dummy"));
            image.Stages.Add(defaultStage);
            var compiled = image.Compile();
            compiled.Should().Be(expected);
        }
    }
}