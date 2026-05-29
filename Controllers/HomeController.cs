using System.Diagnostics;
using dndsitgen.Models;
using dndsitgen.Serveces;
using dndsitgen.Services;
using Microsoft.AspNetCore.Mvc;

namespace dndsitgen.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CreaturesService _service;

        public HomeController(ILogger<HomeController> logger, CreaturesService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _service.getCount());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
