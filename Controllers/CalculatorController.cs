using System.Text;
using System.Text.Json;
using dndsitgen.Models;
using dndsitgen.Serveces;
using dndsitgen.Serveces.Scenaries;
using dndsitgen.Services;
using dndsitgen.Utils;
using Microsoft.AspNetCore.Mvc;
public class CalculatorController : Controller
{
    private readonly CreatureCalculatorService calc;
    private readonly CreaturesService creaturesService;
    private readonly GroqService groq;

    public CalculatorController(CreatureCalculatorService calc, CreaturesService creaturesService, GroqService groqService)
    {
        this.calc = calc;
        this.creaturesService = creaturesService;
        this.groq = groqService;
    }


    [HttpGet("/calculator")]
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


    [HttpPost ("/calculator")]
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

        StringBuilder prompt = new StringBuilder(
            $"Придумай и коротко опиши D&D сцену (только обстоятельста и окружение, не надо рассказывать про ход битвы, название существ пиши на русском языке, отчёт о выполнении писать НЕ надо). Сценарий: {model.ScenaryDesc[model.SelectedScenary]}. Противники (способности существ описывай норативно): "
        );

                for (int i = 0; i < model.Monsters.Length; i++)
                {
                    prompt.Append($"{model.K[i]} шт — {model.Monsters[i].getEffectiveDescShort()}, ");
                }

        model.groqAnswer = await groq.AskAsync(prompt.ToString());

        Console.WriteLine("PROMPT:\n" + prompt.ToString());
        Console.WriteLine("ANSWER:\n" + model.groqAnswer);


        HttpContext.Session.SetString("LastModel", JsonSerializer.Serialize(model));

        return View(model);

    }



}
