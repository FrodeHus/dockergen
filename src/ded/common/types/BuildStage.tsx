class BuildStage {
  BaseImage: FromCommand;
  Name: string;
  Steps: BuildStep[];

  Compile(): string {
    const baseImage = this.BaseImage.Compile();
    return `${baseImage} as ${this.Name}`;
  }
}
