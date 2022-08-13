class BuildStep {
  Name: string;
  Note: string;
  Commands: IDockerCommand[];
  Compile(): string {
    throw new Error("Method not implemented.");
  }
}
