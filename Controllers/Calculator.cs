using Microsoft.AspNetCore.Mvc;
using dndsitgen.Serveces;
using dndsitgen.Serveces.Scenaries;
using dndsitgen.Utils;
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
        int[] k = model.KInput
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => int.Parse(s.Trim()))
            .OrderBy(x => x)
            .ToArray();


        var scenary = ScenaryFactory.Create(model.SelectedScenary);

        float[] raw = calc.getRawCRs(model.Target, k, scenary);
        float[] std = CRStandardizer.toStandart(raw);

        model.K = k;
        model.RawCR = raw;
        model.StandartCR = std;

        model.CurrentComplexity = calc.getComplexity(raw, k);
        model.StandartComplexity = calc.getComplexity(std, k);

        return View(model);
    }

}
