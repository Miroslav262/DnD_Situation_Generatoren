using System.Text.Json;
using dndsitgen.Models;
using dndsitgen.Serveces;
using dndsitgen.Serveces.Scenaries;
using dndsitgen.Utils;
using Microsoft.AspNetCore.Mvc;
public class CalculatorController : Controller
{
    private readonly CreatureCalculatorService calc;
    private readonly CreaturesService creaturesService;

    public CalculatorController(CreatureCalculatorService calc, CreaturesService creaturesService)
    {
        this.calc = calc;
        this.creaturesService = creaturesService;
    }


    [HttpGet]
    public IActionResult Index()
    {
        var saved = HttpContext.Session.GetString("LastModel");
        if (saved != null)
        {
            var model = JsonSerializer.Deserialize<CalculatorViewModel>(saved);
            return View(model);
        }

        return View(new CalculatorViewModel());
    }


    [HttpPost]
    public async Task<IActionResult> Index(CalculatorViewModel model)
    {
        HttpContext.Session.Remove("LastModel");

        if (!string.IsNullOrWhiteSpace(model.HeroCRInput) &&
            !string.IsNullOrWhiteSpace(model.HeroKInput))
        {
            int[] heroK = model.HeroKInput
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.Parse(s.Trim()))
                .OrderBy(x => x)
                .ToArray();

            float[] heroCR = model.HeroCRInput
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => float.Parse(s.Trim()))
                .ToArray();

            model.HeroK = heroK;
            model.HeroCR = heroCR;

            model.HeroComplexity = calc.getHeroesComplexity(heroCR, heroK);
        }
        else
        {
            model.HeroComplexity = 1;
        }


        if (!string.IsNullOrWhiteSpace(model.KInput))
        {
            var scenary = ScenaryFactory.Create(model.SelectedScenary);

            int[] k = model.KInput
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.Parse(s.Trim()))
                .ToArray();

            model.K = k;

            if (k.Length > 0)
            {
                float[] raw = calc.getRawCRs(model.HeroComplexity, k, scenary);
                float[] std = CRStandardizer.toStandart(raw);

                model.K = k;
                model.RawCR = raw;
                model.StandartCR = std;

                model.Monsters = new CreatureModel[k.Length];
                for (int i = 0; i < k.Length; i++)
                {

                    model.Monsters[i] = await creaturesService.getRandomCreatureByCR(std[i]);
                }

                model.CurrentComplexity = calc.getComplexity(raw, k);
                model.StandartComplexity = calc.getComplexity(std, k);
            }


        }
        HttpContext.Session.SetString("LastModel", JsonSerializer.Serialize(model));

        return View(model);
    }



}
