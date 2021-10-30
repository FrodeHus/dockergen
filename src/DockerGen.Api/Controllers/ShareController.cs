using DockerGen.Container;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace DockerGen.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShareController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<ShareController> _log;
        private readonly ContainerService _containerService;

        public ShareController(IDistributedCache cache, ILogger<ShareController> log, ContainerService containerService)
        {
            _cache = cache;
            _log = log;
            _containerService = containerService;
        }

        [HttpGet("/{id}", Name = "GetDockerfile")]
        public async Task<IActionResult> GetQuickShareDockerfileAsync(string id)
        {
            _log.LogInformation($"Retrieving quick link: {id}");
            try
            {
                var actualId = GuidEncoder.Decode(id);
                await _containerService.EnsureRecipesLoaded();
                var dockerfile = await _cache.GetStringAsync(actualId.ToString());
                if (dockerfile == null)
                {
                    _log.LogInformation($"No value in cache for {id}");
                    return NotFound();
                }
                var containerImage = JsonSerializer.Deserialize<ContainerImage>(dockerfile);
                return Ok(containerImage);
            }catch(Exception ex)
            {
                _log.LogInformation(ex.Message);
                return StatusCode(500);
            }
        }
        [HttpPost("quick", Name = "ShareDockerfile")]
        public async Task<IActionResult> ShareDockerfileAsync([FromBody] ContainerImage dockerfile)
        {
            try
            {
                var id = Guid.NewGuid();
                await _containerService.EnsureRecipesLoaded();
                var serialized = JsonSerializer.Serialize(dockerfile);
                await _cache.SetStringAsync(id.ToString(), serialized);
                return Created("/" + GuidEncoder.Encode(id), null);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Failed to create quick share link");
                return BadRequest("Could not read provided Dockerfile");
            }
        }
    }
}
