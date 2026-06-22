using dndsitgen.Models;
using dndsitgen.Repository;
using Microsoft.AspNetCore.Mvc;

namespace dndsitgen.Controllers
{
    [ApiController]
    [Route("user/{name}/collections/{sceneId}/creatures")]
    public class CollectionCreatureController : Controller
    {

        private readonly CreatureRepository repository;
        private readonly UserRepository userRepository;

        public CollectionCreatureController(CreatureRepository repository, UserRepository userRepository) {
            this.repository = repository;
            this.userRepository = userRepository;
        }
        [HttpGet]
        public async Task<IActionResult> getCreatures(
        [FromRoute] string name,
        [FromRoute] int sceneId)
        {
            int? userId = await userRepository.getIdByNameAsync(name);
            if (userId == null)
                return NotFound("User not found");

            var creatures = await repository.GetCreaturesInScene(sceneId);

            return Ok(new { creatures });
        }

        [HttpPost]
        public async Task<IActionResult> addCreature(
            [FromRoute] string name,
            [FromRoute] int sceneId,
            [FromBody] CreatureCollectionRatio dto)
        {
            int? userId = await userRepository.getIdByNameAsync(name);
            if (userId == null)
                return NotFound("User not found");

            bool ok = await repository.AddCreatureToScene(sceneId, dto.CreatureId, dto.Count);

            return ok ? Ok("Added") : StatusCode(500, "Failed to add creature");
        }

        [HttpPatch("{creatureId}")]
        public async Task<IActionResult> updateCreature(
            [FromRoute] string name,
            [FromRoute] int sceneId,
            [FromRoute] int creatureId,
            [FromBody] CreatureCollectionRatio dto)
        {
            int? userId = await userRepository.getIdByNameAsync(name);
            if (userId == null)
                return NotFound("User not found");

            bool ok = await repository.UpdateCreatureCount(sceneId, creatureId, dto.Count);

            return ok ? Ok("Updated") : StatusCode(500, "Failed to update creature");
        }

        [HttpDelete("{creatureId}")]
        public async Task<IActionResult> deleteCreature(
            [FromRoute] string name,
            [FromRoute] int sceneId,
            [FromRoute] int creatureId)
        {
            int? userId = await userRepository.getIdByNameAsync(name);
            if (userId == null)
                return NotFound("User not found");

            bool ok = await repository.DeleteCreatureFromScene(sceneId, creatureId);

            return ok ? Ok("Deleted") : StatusCode(500, "Failed to delete creature");
        }

        [HttpPatch("{creatureId}/change")]
        public async Task<IActionResult> changeCreature(
        [FromRoute] string name,
        [FromRoute] int sceneId,
        [FromRoute] int creatureId,
        [FromBody] ChangeCreatureDto dto)
        {
            int? userId = await userRepository.getIdByNameAsync(name);
            if (userId == null)
                return NotFound("User not found");

            bool ok = await repository.ChangeCreature(sceneId, creatureId, dto.NewCreatureId);

            return ok
                ? Ok("Creature changed successfully")
                : StatusCode(500, "Unexpected error while changing creature");
        }

    }
}
