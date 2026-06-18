using System;
using System.Threading.Tasks;
using dndsitgen.Models;
using dndsitgen.Serveces;
using dndsitgen.Repository;
using Microsoft.AspNetCore.Mvc;

namespace dndsitgen.Controllers
{
    public class CreatureController : Controller
    {
        private readonly CreaturesService _service;
        private readonly CreatureRepository repository;
        private int? maxId = null;
        public CreatureController(CreaturesService service, CreatureRepository creatureRepository) {
            _service = service;
            this.repository = creatureRepository;


        }

        [HttpGet("/creature/random_creature")]
        public async Task<IActionResult> RandomCreature() {
            if (maxId == null) {
                maxId = await _service.getCount();
            }
            int randomId = Random.Shared.Next(1, maxId.Value + 1);
            return View("~/Views/Creature/Creature.cshtml", await _service.getCreature(randomId));
        }
        [HttpPost("/creature")]
        public async Task<IActionResult> Creature(CreatureModel creatureModel)
        {
            return View("~/Views/Creature/Creature.cshtml", creatureModel);
        }

        [HttpGet("/creature/{key}")]
        public async Task<IActionResult> CreatureByKey(string key)
        {
            return View("~/Views/Creature/Creature.cshtml", await _service.getCreatureByKey(key));
        }
        [HttpGet("/test")]
        public IActionResult Test() {
            this.repository.Test();
            return Ok("done");
        }

    }
}
