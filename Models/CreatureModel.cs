using System.Text.Json.Serialization;
namespace dndsitgen.Models;
public class CreatureModel
{
    public string? key { get; set; }
    public string? name { get; set; }
    public DocumentInfo? document { get; set; }
    public CreatureType? type { get; set; }
    public CreatureSize? size { get; set; }
    public double? challenge_rating { get; set; }
    public int? proficiency_bonus { get; set; }
    public CreatureSpeed? speed { get; set; }
    public CreatureSpeedAll? speed_all { get; set; }
    public string? category { get; set; }
    public string? subcategory { get; set; }
    public string? alignment { get; set; }
    public CreatureLanguages? languages { get; set; }
    public int armor_class { get; set; }
    public string? armor_detail { get; set; }
    public int hit_points { get; set; }
    public string? hit_dice { get; set; }
    public int? experience_points { get; set; }
    public AbilityScores? ability_scores { get; set; }
    public AbilityModifiers? modifiers { get; set; }
    public int initiative_bonus { get; set; }
    public SavingThrows? saving_throws { get; set; }
    public SavingThrows? saving_throws_all { get; set; }
    public SkillBonuses? skill_bonuses { get; set; }
    public SkillBonuses? skill_bonuses_all { get; set; }
    public int passive_perception { get; set; }
    public ResistancesAndImmunities? resistances_and_immunities { get; set; }
    public int? darkvision_range { get; set; }
    public int? blindsight_range { get; set; }
    public int? tremorsense_range { get; set; }
    public int? truesight_range { get; set; }
    public List<CreatureAction>? actions { get; set; }
    public List<CreatureTrait>? traits { get; set; }

    public string getEffectiveDescShort()
    {
        var traits = this.traits?.Select(t => t.name).Take(2);
        var actions = this.actions?.Select(a => a.name).Take(2);

        return $"{name}, {size?.name} {type?.name}";
    }


}

public class DocumentInfo
{
    public string? name { get; set; }
    public string? key { get; set; }
    public string? type { get; set; }
    public string? display_name { get; set; }
    public Publisher? publisher { get; set; }
    public GameSystem? gamesystem { get; set; }
    public string? permalink { get; set; }
}

public class Publisher
{
    public string? name { get; set; }
    public string? key { get; set; }
}

public class GameSystem
{
    public string? name { get; set; }
    public string? key { get; set; }
}

public class CreatureType
{
    public string? name { get; set; }
    public string? key { get; set; }
}

public class CreatureSize
{
    public string? name { get; set; }
    public string? key { get; set; }
}

public class CreatureSpeed
{
    public int? walk { get; set; }
    public string? unit { get; set; }
}

public class CreatureSpeedAll
{
    public string? unit { get; set; }
    public int? walk { get; set; }
    public int? crawl { get; set; }
    public bool? hover { get; set; }
    public int? fly { get; set; }
    public int? burrow { get; set; }
    public int? climb { get; set; }
    public int? swim { get; set; }
}

public class CreatureLanguages
{
    public string? as_string { get; set; }
    public List<LanguageData>? data { get; set; }
}

public class LanguageData
{
    public string? name { get; set; }
    public string? key { get; set; }
    public string? desc { get; set; }
}

public class AbilityScores
{
    public int strength { get; set; }
    public int dexterity { get; set; }
    public int constitution { get; set; }
    public int intelligence { get; set; }
    public int wisdom { get; set; }
    public int charisma { get; set; }
}

public class AbilityModifiers
{
    public int strength { get; set; }
    public int dexterity { get; set; }
    public int constitution { get; set; }
    public int intelligence { get; set; }
    public int wisdom { get; set; }
    public int charisma { get; set; }
}

public class SavingThrows
{
    public int strength { get; set; }
    public int dexterity { get; set; }
    public int constitution { get; set; }
    public int intelligence { get; set; }
    public int wisdom { get; set; }
    public int charisma { get; set; }
}

public class SkillBonuses
{
    public int acrobatics { get; set; }
    public int animal_handling { get; set; }
    public int arcana { get; set; }
    public int athletics { get; set; }
    public int deception { get; set; }
    public int history { get; set; }
    public int insight { get; set; }
    public int intimidation { get; set; }
    public int investigation { get; set; }
    public int medicine { get; set; }
    public int nature { get; set; }
    public int perception { get; set; }
    public int performance { get; set; }
    public int persuasion { get; set; }
    public int religion { get; set; }
    public int sleight_of_hand { get; set; }
    public int stealth { get; set; }
    public int survival { get; set; }
}

public class ResistancesAndImmunities
{
    public List<DamageEntry>? damage_immunities { get; set; }
    public List<DamageEntry>? damage_resistances { get; set; }
    public List<DamageEntry>? damage_vulnerabilities { get; set; }
    public List<DamageEntry>? condition_immunities { get; set; }
}


public class CreatureAction
{
    public string? name { get; set; }
    public string? desc { get; set; }
    public List<CreatureAttack>? attacks { get; set; }
    public string? action_type { get; set; }
    public int? order_in_statblock { get; set; }
    public int? legendary_action_cost { get; set; }
    public object? limited_to_form { get; set; }
    public UsageLimits? usage_limits { get; set; }
}

public class CreatureAttack
{
    public string? name { get; set; }
    public string? attack_type { get; set; }
    public int? to_hit_mod { get; set; }
    public int? reach { get; set; }
    public int? range { get; set; }
    public int? long_range { get; set; }
    public bool target_creature_only { get; set; }
    public int? damage_die_count { get; set; }
    public string? damage_die_type { get; set; }
    public int? damage_bonus { get; set; }
    public DamageType? extra_damage_type { get; set; }
    public string? distance_unit { get; set; }
}


public class DamageType
{
    public string? name { get; set; }
    public string? key { get; set; }
}

public class UsageLimits
{
    public string? type { get; set; }
    public int? param { get; set; }
}

public class CreatureTrait
{
    public string? name { get; set; }
    public string? desc { get; set; }
}
public class DamageEntry
{
    public string? key { get; set; }
    public string? name { get; set; }
 }

