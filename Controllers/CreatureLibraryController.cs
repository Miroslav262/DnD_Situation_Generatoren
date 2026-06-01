using System;
using System.Threading.Tasks;
using dndsitgen.Models;
using dndsitgen.Serveces;
using Microsoft.AspNetCore.Mvc;

namespace dndsitgen.Controllers
{
    public class CreatureLibraryController : Controller
    {
        private readonly CreaturesService _service;
        private int? maxId = null;
        public CreatureLibraryController(CreaturesService service) {
            _service = service;

        }
        public async Task<IActionResult> Index()
        {

            int count = await _service.getCount();
            return View("~/Views/Home/Index.cshtml", count);
        }
        [HttpGet("/creature/random_creature")]
        public async Task<IActionResult> RandomCreature() {
            if (maxId == null) {
                maxId = await _service.getCount();
            }
            int randomId = Random.Shared.Next(1, maxId.Value + 1);
            return View("~/Views/Home/Creature.cshtml", await _service.getCreature(randomId));
        }
        [HttpPost("/creature")]
        public async Task<IActionResult> Creature(CreatureModel creatureModel)
        {
            return View("~/Views/Home/Creature.cshtml", creatureModel);
        }

        [HttpGet("/creature/{key}")]
        public async Task<IActionResult> CreatureByKey(string key)
        {
            return View("~/Views/Home/Creature.cshtml", await _service.getCreatureByKey(key));
        }

    }
}
