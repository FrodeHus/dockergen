using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerGen.Container
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class IgnoreInstructionAttribute : Attribute
    {
        public string Reason { get; set; }
    }
}
