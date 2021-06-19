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

        [Fact]
        public void Generate_Multistage_Image()
        {
            const string expected = @"FROM dotnet:v1 AS build
RUN dotnet build stuff

FROM dotnet-sdk:v1
COPY --from=build . .
ENTRYPOINT [""dotnet"", ""stuff.dll""]";
            var buildStage = new BuildStage();
            buildStage.BaseImage = "dotnet:v1 AS build";
            buildStage.Instructions.Add(new RunInstruction("dotnet build stuff"));
            var runtimeStage = new BuildStage();
            runtimeStage.BaseImage = "dotnet-sdk:v1";
            runtimeStage.Instructions.Add(new CopyInstruction(".", ".") { Location = "build" });
            runtimeStage.Instructions.Add(new EntryPointInstruction("dotnet", "stuff.dll"));
            var image = new ContainerImage();
            image.Stages.Add(buildStage);
            image.Stages.Add(runtimeStage);
            var compiled = image.Compile();
            compiled.Should().Be(expected);
        }
    }
}