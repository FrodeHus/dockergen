using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerGen.Container
{
    public class ParseInstructionException : Exception
    {
        public ParseInstructionException() : base()
        {
        }

        public ParseInstructionException(string message) : base(message)
        {
        }

        public ParseInstructionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
