using System.Text.Json.Serialization;
namespace dndsitgen.Models;
public class CreatureModel
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public int SizeId { get; set; }

    public int TypeId { get; set; }

    public int AlignmentId { get; set; }

    public int Ac { get; set; }

    public int? HpDefault { get; set; }

    public int HpDice { get; set; }

    public int HpDiceCount { get; set; }

    public int HpAddition { get; set; }

    public int Passive { get; set; }

    public int CrId { get; set; }

    public int? SourceId { get; set; }
    public int Strength { get; set; }

    public int Dexterity { get; set; }

    public int Constitution { get; set; }

    public int Intelligence { get; set; }

    public int Wisdom { get; set; }

    public int Charisma { get; set; }
    public int SavingStrength { get; set; }

    public int SavingDexterity { get; set; }

    public int SavingConstitution { get; set; }

    public int SavingIntelligence { get; set; }

    public int SavingWisdom { get; set; }

    public int SavingCharisma { get; set; }
    public int Acrobatics { get; set; }

    public int AnimalHandling { get; set; }

    public int Arcana { get; set; }

    public int Athletics { get; set; }

    public int Deception { get; set; }

    public int History { get; set; }

    public int Insight { get; set; }

    public int Intimidation { get; set; }

    public int Investigation { get; set; }

    public int Medicine { get; set; }

    public int Nature { get; set; }

    public int Perception { get; set; }

    public int Performance { get; set; }

    public int Persuasion { get; set; }

    public int Religion { get; set; }

    public int SleightOfHand { get; set; }

    public int Stealth { get; set; }

    public int Survival { get; set; }
    public int SpeedUnitId { get; set; }

    public int? Walk { get; set; }

    public int? Crawl { get; set; }

    public int? Hover { get; set; }

    public int? Fly { get; set; }

    public int? Burrow { get; set; }

    public int? Climb { get; set; }

    public int? Swim { get; set; }
    public int CreatureSensesUnitId { get; set; }

    public int? DarkvisionRange { get; set; }

    public int? BlindsightRange { get; set; }

    public int? TremorsenseRange { get; set; }

    public int? TruesightRange { get; set; }
    public List<int> LanguageIds { get; set; } = [];

    public List<int> TraitIds { get; set; } = [];

    public List<int> ActionIds { get; set; } = [];

    public List<int> LegendaryIds { get; set; } = [];

    public List<int> BiomeIds { get; set; } = [];

    public List<int> DamageImmunityIds { get; set; } = [];

    public List<int> DamageResistanceIds { get; set; } = [];

    public List<int> DamageVulnerabilityIds { get; set; } = [];

    public List<int> ConditionImmunityIds { get; set; } = [];

}


