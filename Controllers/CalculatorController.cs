using dndsitgen.Models;
using dndsitgen.Serveces;
using dndsitgen.Serveces.Scenaries;
using dndsitgen.Services;
using dndsitgen.Utils;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("calculator")]
public class CalculatorController : Controller
{
    private readonly CreatureCalculatorService calc;
    public CalculatorController(CreatureCalculatorService calc)
    {
        this.calc = calc;
    }

    [HttpPost]
    public IActionResult Calculate([FromBody] CalcUnit model)
    {
        if (model == null ||
            model.hero_levels == null ||
            model.count_heros == null ||
            model.hero_levels.Length != model.count_heros.Length ||
            model.enemy_count == null)
        {
            return BadRequest("Incorrect data");
        }

        var scenary = ScenaryFactory.Create(model.scenary);

        var response = new CalcResponse
        {
            HeroComplexity = calc.getHeroesComplexity(model.hero_levels, model.count_heros),
        };

        response.RawCR = calc.getRawCRs((float)response.HeroComplexity, model.count_heros, scenary);
        response.StandartCR = CRStandardizer.toStandart(response.RawCR);

        response.CurrentComplexity = calc.getComplexity(response.RawCR, model.count_heros);
        response.StandartComplexity = calc.getComplexity(response.StandartCR, model.count_heros);

        return Ok(response);
    }
}
