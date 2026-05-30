using dndsitgen.Models;
using dndsitgen.Serveces;
using dndsitgen.Serveces.Scenaries;
using dndsitgen.Utils;
using Microsoft.AspNetCore.Mvc;
public class CalculatorController : Controller
{
    private readonly CreatureCalculatorService calc;

    public CalculatorController(CreatureCalculatorService calc)
    {
        this.calc = calc;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new CalculatorViewModel());
    }

    [HttpPost]
    public IActionResult Index(CalculatorViewModel model)
    {
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

                model.CurrentComplexity = calc.getComplexity(raw, k);
                model.StandartComplexity = calc.getComplexity(std, k);
            }

        }

        return View(model);
    }



}
