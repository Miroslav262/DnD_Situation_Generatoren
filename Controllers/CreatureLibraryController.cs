using System.Threading.Tasks;
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

        public async Task<IActionResult> Creature() {
            if (maxId == null) {
                maxId = await _service.getCount();
            }
            int randomId = Random.Shared.Next(1, maxId.Value + 1);
            return View("~/Views/Home/Creature.cshtml", await _service.getCreature(randomId));
        }
    }
}
