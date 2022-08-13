interface IDockerCommand {
  Name: string;
  HelpText: string;
  Command: string;

  Compile(): string;
}
