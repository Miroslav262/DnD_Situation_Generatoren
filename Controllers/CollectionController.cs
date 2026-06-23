using dndsitgen.Models;
using dndsitgen.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dndsitgen.Controllers
{
    [Authorize]
    [ApiController]
    [Route("user/{name}/collections")]
    public class CollectionController : Controller
    {
        private readonly CollectionRepository collectionRepository;
        private readonly UserRepository userRepository;

        public CollectionController(CollectionRepository collectionRepository, UserRepository userRepository)
        {
            this.collectionRepository = collectionRepository;
            this.userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> getAllCollections([FromRoute] string name)
        {
            int? id = await userRepository.getIdByNameAsync(name);
            if (id == null)
                return NotFound("User not found");

            BattleScene[]? scenes = await collectionRepository.getBattleScenesByUserIdAsync(id.Value);

            return Ok(new { collections = scenes ?? Array.Empty<BattleScene>() });
        }

        [HttpPost]
        public async Task<IActionResult> createCollection(
            [FromRoute] string name,
            [FromBody] BattleScene battleScene)
        {
            int? id = await userRepository.getIdByNameAsync(name);
            if (id == null)
                return NotFound("User not found");

            bool ok = await collectionRepository.createBattleScene(battleScene, id.Value);

            return ok
                ? Ok("Success")
                : BadRequest("Failed to create battle scene");
        }

        [HttpPatch("{sceneId}")]
        public async Task<IActionResult> updateCollection(
            [FromRoute] string name,
            [FromRoute] int sceneId,
            [FromBody] BattleScene battleScene)
        {
            int? id = await userRepository.getIdByNameAsync(name);
            if (id == null)
                return NotFound("User not found");

            battleScene.Id = sceneId;

            bool ok = await collectionRepository.updateBattleScene(battleScene);

            return ok
                ? Ok("Successful update")
                : StatusCode(500, "Unexpected error while updating scene");
        }

        [HttpDelete("{sceneId}")]
        public async Task<IActionResult> deleteCollection(
            [FromRoute] string name,
            [FromRoute] int sceneId)
        {
            int? id = await userRepository.getIdByNameAsync(name);
            if (id == null)
                return NotFound("User not found");

            bool ok = await collectionRepository.deleteBattleScene(sceneId);

            return ok
                ? Ok("Successful delete")
                : StatusCode(500, "Unexpected error while deleting scene");
        }
    }
}
