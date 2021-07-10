using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerGen.Components
{
    public class DragEventArgs : MouseEventArgs
    {
        public DataTransfer DataTransfer { get; set; }

    }
}
