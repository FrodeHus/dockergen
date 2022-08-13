class FromCommand implements IDockerCommand {
  Name: string = "FROM";
  HelpText: string = "Sets the base image for the current build stage";
  Command: string = "FROM";
  ImageRepository: string;
  Tag: string;
  Compile(): string {
    return `${this.Command} ${this.ImageRepository}:${this.Tag}`;
  }
}
