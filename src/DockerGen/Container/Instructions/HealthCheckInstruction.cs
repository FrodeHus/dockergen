using System.Text;

namespace DockerGen.Container
{
    public class HealthCheckInstruction : Instruction
    {
        public HealthCheckInstruction()
        {

        }
        public override string Description => throw new System.NotImplementedException();

        public override string Prefix => "HEALTHCHECK";
        public override string DisplayName => "Check if the container is still working";
        public int Interval { get; set; } = 30;
        
        public int Timeout {get;set;} = 30;
        public int StartPeriod {get;set;} = 0;
        public int Retries {get;set;} = 3;
        public bool Disabled {get;set;} 
        protected override void CompileArguments(StringBuilder builder)
        {
            if(Disabled){
                builder.AppendLine("NONE");
                return;
            }
            
        }

        public static implicit operator HealthCheckInstruction(string value)
        {
            return null;
        }
    }
}