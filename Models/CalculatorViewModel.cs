
namespace dndsitgen.Models
{
    public class CalculatorViewModel
    {
        public string KInput { get; set; } = "";
        public float Target { get; set; }
        public string SelectedScenary { get; set; } = "Minions";

        public float[]? RawCR { get; set; }
        public float[]? StandartCR { get; set; }
        public int[]? K { get; set; }
        public float CurrentComplexity { get; set; }
        public float StandartComplexity { get; set; }
        public string HeroCRInput { get; set; } = "";
        public string HeroKInput { get; set; } = "";

        public float[]? HeroCR { get; set; }
        public int[]? HeroK { get; set; }
        public float HeroComplexity { get; set; }

        public List<string> Scenaries { get; set; } = new()
        {
            "Minions",
            "Boss",
            "Linear",
            "Uniform",
            "Root",
            "SuperBoss"
        };
        public Dictionary<string, string> ScenaryLocalization { get; set; } = new Dictionary<string, string>
            {
                { "Minions", "Миньоны" },
                { "Boss", "Босс" },
                { "Uniform", "Равномерный" },
                { "Root", "Корень" },
                { "Linear", "Линейный" },
                { "SuperBoss", "Супербосс" }
            };

        public Dictionary<string, string> ScenaryDesc { get; set; } = new Dictionary<string, string>
        {
            { "Minions", "массовая схватка с большим количеством слабых противников" },
            { "Boss", "битва с сильным противником, вокруг которого строится вся сцена" },
            { "Uniform", "сбалансированное столкновение с равными по силе врагами" },
            { "Root", "столкновение с врагами незначительно различающихся по силе" },
            { "Linear", "постепенно нарастающая по сложности битва, где враги становятся сильнее" },
            { "SuperBoss", "эпическая битва с главным, чрезвычайно мощным боссом" }
        };

        public CreatureModel[]? Monsters { get; set; }

        public string? groqAnswer { get; set; }


    }
}

