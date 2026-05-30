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
    public List<string> Scenaries { get; set; } = new()
    {
        "Minions",
        "Boss",
        "Uniform",
        "Root"
    };
}
