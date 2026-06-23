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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View();
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
