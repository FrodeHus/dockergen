using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerGen.Api.Controllers
{
    [ApiController]
    [Route("{controller}")]
    public class ShareController : ControllerBase
    {
        [HttpPost(Name = "ShareDockerfile")]
        public IAsyncResult ShareDockerfileAsync([FromBody] string dockerfile)
        {
            throw new NotImplementedException();
        }
    }
}
