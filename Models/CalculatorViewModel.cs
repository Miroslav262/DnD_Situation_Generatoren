
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
            "Uniform",
            "Root"
        };
    }
}

