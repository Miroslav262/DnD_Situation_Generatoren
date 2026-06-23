
using dndsitgen.Serveces.Scenaries;

namespace dndsitgen.Models
{
    public class CalcUnit
    {
        public int[]? hero_levels { get; set; }
        public int[]? count_heros { get; set; }
        public int[]? enemy_count { get; set; }
        public ScenaryEnum scenary { get; set; }

    }

    public class CalcResponse
    {
        public float? HeroComplexity { get; set; }
        public float? CurrentComplexity { get; set; }
        public float? StandartComplexity { get; set; }
        public float[]? RawCR { get; set; }
        public float[]? StandartCR { get; set; }
        
    }
}

