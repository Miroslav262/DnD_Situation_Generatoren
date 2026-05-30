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
        private readonly GroqService groqService;

        public HomeController(ILogger<HomeController> logger, CreaturesService service, GroqService groqService)
        {
            _logger = logger;
            _service = service;
            this.groqService = groqService;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _service.getCount());
        }

        [HttpGet("/groq_test")]
        public async Task<IActionResult> GroqTest()
        {
            try
            {
                string answer = await groqService.AskAsync("Расскажи кратко, что такое D&D?");

                return Json(new
                {
                    ok = true,
                    answer = answer
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    ok = false,
                    error = ex.Message
                });
            }
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
