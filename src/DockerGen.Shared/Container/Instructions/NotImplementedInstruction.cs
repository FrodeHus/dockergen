using DockerGen.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerGen.Container
{
    [IgnoreInstruction(Reason = "Is not an actual Docker instruction")]
    public class NotImplementedInstruction : IDockerInstruction
    {
        private string _prefix;
        public NotImplementedInstruction(string prefix)
        {
            _prefix = prefix;
        }

        public  string Prefix => _prefix;

        public  string DisplayName => Prefix;

        public  string Description => "This instruction is not supported or has not yet been implemented";

        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        public  string Compile()
        {
            return $"#{Prefix} is not implemented";
        }
    }
}
