using System.Text;

namespace DockerGen.Container
{
    public class HealthCheckInstruction : Instruction
    {
        public HealthCheckInstruction()
        {

        }
        public override string Description => "Runs a command periodically to check if the container is still working.";

        public override string Prefix => "HEALTHCHECK";
        public override string DisplayName => "Configure health check";
        public int Interval { get; set; } = 30;

        public int Timeout { get; set; } = 30;
        public int StartPeriod { get; set; } = 0;
        public int Retries { get; set; } = 3;
        public bool Disabled { get; set; }
        public string Command { get; set; } = "curl -f http://localhost/ || exit 1";
        protected override void CompileArguments(StringBuilder builder)
        {
            if (Disabled)
            {
                builder.AppendLine("NONE");
                return;
            }
            builder.Append($"--interval={Interval}s ");
            builder.Append($"--timeout={Timeout}s ");
            builder.Append($"--start-period={StartPeriod}s ");
            builder.Append($"--retries={Retries}s ");
            builder.AppendLine(Command);
        }

        public static implicit operator HealthCheckInstruction(string value)
        {
            return null;
        }
    }
}