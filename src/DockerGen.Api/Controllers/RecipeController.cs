using DockerGen.Container;
using DockerGen.Container.Recipes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerGen.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly ContainerService _containerService;

        public RecipeController(ContainerService containerService)
        {
            _containerService = containerService;
        }
        [HttpGet(Name ="GetRecipesAsync")]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipesAsync()
        {
            var recipes = await _containerService.GetValidRecipesAsync();
            return Ok(recipes);
        }

        [HttpPost(Name = "AddRecipeAsync")]
        public IActionResult AddRecipeAsync()
        {
            return BadRequest();
        }

        [HttpPut("{recipeId}")]
        public IActionResult UpdateRecipeAsync(string recipeId)
        {
            return BadRequest();
        }

    }
}
