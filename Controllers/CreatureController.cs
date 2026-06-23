using System;
using System.Reflection;
using System.Threading.Tasks;
using dndsitgen.Models;
using dndsitgen.Repository;
using dndsitgen.Serveces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dndsitgen.Controllers
{
    [Authorize]
    [ApiController]
    [Route("creatures")]
    public class CreatureController : Controller
    {
        private readonly CreatureRepository repository;
        public CreatureController(CreatureRepository creatureRepository) {
            this.repository = creatureRepository;
        }

        [HttpGet("random")]
        public async Task<IActionResult> getRandomCreature() {
            CreatureModel? model = await repository.getRandomCreatureAsync();
            if (model != null)
            {
                return Ok(model);
            }
            else
            {
                return StatusCode(500, "Unexpected error");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> getCreatureById([FromRoute]int id) {

            CreatureModel? model = await repository.getByIdAsync(id);
            if (model != null)
            {
                return Ok(model);
            }
            else
            {
                return StatusCode(500, "Unexpected error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> getAllCreatures([FromQuery]CreatureFilter filter)
        {
            CreatureModel[]? models = await repository.getFilteredCreaturesAsync(filter);
            if (models != null)
            {
                return Ok(models);
            }
            else {
                return StatusCode(500, "Unexpected error");
            }

        }
    }
}
