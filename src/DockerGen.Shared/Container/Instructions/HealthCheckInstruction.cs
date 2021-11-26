using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
        public int StartPeriod { get; set; }
        public int Retries { get; set; } = 3;
        public bool Disabled { get; set; }
        public string Command { get; set; }
        protected override void CompileArguments(StringBuilder builder)
        {
            if (Disabled)
            {
                builder.AppendLine("NONE");
                return;
            }
            builder.Append("--interval=").Append(Interval).Append("s ");
            builder.Append("--timeout=").Append(Timeout).Append("s ");
            builder.Append("--start-period=").Append(StartPeriod).Append("s ");
            builder.Append("--retries=").Append(Retries).Append("s ");
            builder.AppendLine(Command);
        }

        public static HealthCheckInstruction ParseFromString(string line)
        {
            if (!line.StartsWith("HEALTHCHECK", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ParseInstructionException("Not a valid prefix: " + line[..line.IndexOf(' ')]);
            }
            var value = line[line.IndexOf(' ')..].Trim();
            var instruction = new HealthCheckInstruction();
            const string optionPattern = @"/(?<option>--(interval|timeout|retries|start-period)=(\d+[m|s]))/gm";
            var optionsMatch = Regex.Matches(value, optionPattern, RegexOptions.IgnoreCase);
            var options = optionsMatch.Select(o =>
            {
                var option = o.Groups["option"];
                var value = option.Captures[3].Value;
                var unit = option.Captures[4].Value.ToLowerInvariant();
                return new HealthCheckOption(
                    option.Captures[2].Value.ToLower(),
                    value,
                    unit);
            });

            foreach (var option in options)
            {
                if (!int.TryParse(option.Value, out var time))
                {
                    throw new ParseInstructionException("Not a valid time: " + option.Value);
                }

                if (option.Unit == "m")
                {
                    time *= 60;
                }

                switch (option.Option)
                {
                    case "interval":
                        instruction.Interval = time;
                        break;
                    case "timeout":
                        instruction.Timeout = time;
                        break;
                    case "start-period":
                        instruction.StartPeriod = time;
                        break;
                    case "retries":
                        instruction.Retries = time;
                        break;
                    default:
                        break;
                }
            }

            instruction.Command = Regex.Replace(value, @"(--(interval|timeout|retries|start-period)=\d+[m|s]?\s?)+", "", RegexOptions.IgnoreCase);
            return instruction;
        }

        private record HealthCheckOption(string Option, string Value, string Unit);
    }

}