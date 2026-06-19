using Microsoft.Extensions.Logging;
using Npgsql;
using dndsitgen.Models;
using Dapper;

namespace dndsitgen.Repository
{
    public class CreatureRepository
    {
        private readonly string con_str;
        private readonly ILogger<CreatureRepository> logger;

        public CreatureRepository(string str, ILogger<CreatureRepository> logger)
        {
            this.con_str = str;
            this.logger = logger;
        }


        public async Task Test()
        {
            await using var connection = new NpgsqlConnection(con_str);

            try
            {
                await connection.OpenAsync();
                logger.LogInformation("Connection opened");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to open connection");
            }
        }

        public async Task<CreatureModel?> GetByIdAsync(long id)
        {
            await using var connection = new NpgsqlConnection(con_str);

            try
            {
                await connection.OpenAsync();

                var creatureEntity = await connection.QueryFirstOrDefaultAsync<CreatureEntity>(
                    @"SELECT *
              FROM ""creature""
              WHERE id = @Id",
                    new { Id = id });

                if (creatureEntity == null)
                    return null;

                var creatureSize = await connection.QueryFirstOrDefaultAsync<CreatureSize>(
                    @"SELECT *
              FROM ""creature_size""
              WHERE id = @Id",
                    new { Id = creatureEntity.SizeId });

                var creatureCr = await connection.QueryFirstOrDefaultAsync<CreatureCr>(
                    @"SELECT *
              FROM ""creature_cr""
              WHERE id = @Id",
                    new { Id = creatureEntity.CrId });

                var creatureSource = creatureEntity.SourceId == null
                    ? null
                    : await connection.QueryFirstOrDefaultAsync<LookupItem>(
                        @"SELECT *
                  FROM ""creature_source""
                  WHERE id = @Id",
                        new { Id = creatureEntity.SourceId });

                var creatureType = await connection.QueryFirstOrDefaultAsync<LookupItem>(
                    @"SELECT *
              FROM ""creature_type""
              WHERE id = @Id",
                    new { Id = creatureEntity.TypeId });

                var creatureAlignment = await connection.QueryFirstOrDefaultAsync<LookupItem>(
                    @"SELECT *
              FROM ""creature_alignment""
              WHERE id = @Id",
                    new { Id = creatureEntity.AlignmentId });

                var speedUnit = await connection.QueryFirstOrDefaultAsync<LookupItem>(
                    @"SELECT *
              FROM ""unit""
              WHERE id = @Id",
                    new { Id = creatureEntity.SpeedUnitId });

                var senseUnit = await connection.QueryFirstOrDefaultAsync<LookupItem>(
                    @"SELECT *
              FROM ""unit""
              WHERE id = @Id",
                    new { Id = creatureEntity.CreatureSensesUnitId });

                var languages = (await connection.QueryAsync<LookupItem>(
                    @"SELECT l.*
              FROM ""creature_languages"" l
              INNER JOIN ""creature_languages_ratio"" clr
                ON clr.language_id = l.id
              WHERE clr.creature_id = @CreatureId",
                    new { CreatureId = creatureEntity.Id }))
                    .ToList();

                var traits = (await connection.QueryAsync<CreatureTrait>(
                    @"SELECT t.*
              FROM ""creature_trait"" t
              INNER JOIN ""creature_trait_ratio"" ctr
                ON ctr.trait_id = t.id
              WHERE ctr.creature_id = @CreatureId",
                    new { CreatureId = creatureEntity.Id }))
                    .ToList();

                var actions = (await connection.QueryAsync<CreatureAction>(
                    @"SELECT a.*
              FROM ""creature_action"" a
              INNER JOIN ""creature_action_ratio"" car
                ON car.action_id = a.id
              WHERE car.creature_id = @CreatureId",
                    new { CreatureId = creatureEntity.Id }))
                    .ToList();

                var legendaries = (await connection.QueryAsync<CreatureLegendary>(
                    @"SELECT l.*
              FROM ""creature_legendary"" l
              INNER JOIN ""creature_legendary_ratio"" clr
                ON clr.legendary_id = l.id
              WHERE clr.creature_id = @CreatureId",
                    new { CreatureId = creatureEntity.Id }))
                    .ToList();

                var biomes = (await connection.QueryAsync<LookupItem>(
                    @"SELECT b.*
              FROM ""creature_biomes"" b
              INNER JOIN ""creature_biome_ratio"" cbr
                ON cbr.biome_id = b.id
              WHERE cbr.creature_id = @CreatureId",
                    new { CreatureId = creatureEntity.Id }))
                    .ToList();

                var damageImmunities = (await connection.QueryAsync<LookupItem>(
                    @"SELECT dt.*
              FROM ""damage_type"" dt
              INNER JOIN ""creature_damage_immunities"" cdi
                ON cdi.damage_type_id = dt.id
              WHERE cdi.creature_id = @CreatureId",
                    new { CreatureId = creatureEntity.Id }))
                    .ToList();

                var damageResistances = (await connection.QueryAsync<LookupItem>(
                    @"SELECT dt.*
              FROM ""damage_type"" dt
              INNER JOIN ""creature_damage_resistances"" cdr
                ON cdr.damage_type_id = dt.id
              WHERE cdr.creature_id = @CreatureId",
                    new { CreatureId = creatureEntity.Id }))
                    .ToList();

                var damageVulnerabilities = (await connection.QueryAsync<LookupItem>(
                    @"SELECT dt.*
              FROM ""damage_type"" dt
              INNER JOIN ""creature_damage_vulnerabilities"" cdv
                ON cdv.damage_type_id = dt.id
              WHERE cdv.creature_id = @CreatureId",
                    new { CreatureId = creatureEntity.Id }))
                    .ToList();

                var conditionImmunities = (await connection.QueryAsync<LookupItem>(
                    @"SELECT ct.*
              FROM ""condition_type"" ct
              INNER JOIN ""creature_condition_immunities"" cci
                ON cci.condition_type_id = ct.id
              WHERE cci.creature_id = @CreatureId",
                    new { CreatureId = creatureEntity.Id }))
                    .ToList();

                return new CreatureModel
                {
                    Id = creatureEntity.Id,
                    Name = creatureEntity.Name,
                    ImageUrl = creatureEntity.ImageUrl,
                    Description = creatureEntity.Description,

                    Size = creatureSize,
                    Type = creatureType,
                    Alignment = creatureAlignment,
                    ChallengeRating = creatureCr,
                    Source = creatureSource,

                    Ac = creatureEntity.Ac,
                    HpDefault = creatureEntity.HpDefault,
                    HpDice = creatureEntity.HpDice,
                    HpDiceCount = creatureEntity.HpDiceCount,
                    HpAddition = creatureEntity.HpAddition,
                    Passive = creatureEntity.Passive,

                    Strength = creatureEntity.Strength,
                    Dexterity = creatureEntity.Dexterity,
                    Constitution = creatureEntity.Constitution,
                    Intelligence = creatureEntity.Intelligence,
                    Wisdom = creatureEntity.Wisdom,
                    Charisma = creatureEntity.Charisma,

                    SavingStrength = creatureEntity.SavingStrength,
                    SavingDexterity = creatureEntity.SavingDexterity,
                    SavingConstitution = creatureEntity.SavingConstitution,
                    SavingIntelligence = creatureEntity.SavingIntelligence,
                    SavingWisdom = creatureEntity.SavingWisdom,
                    SavingCharisma = creatureEntity.SavingCharisma,

                    Acrobatics = creatureEntity.Acrobatics,
                    AnimalHandling = creatureEntity.AnimalHandling,
                    Arcana = creatureEntity.Arcana,
                    Athletics = creatureEntity.Athletics,
                    Deception = creatureEntity.Deception,
                    History = creatureEntity.History,
                    Insight = creatureEntity.Insight,
                    Intimidation = creatureEntity.Intimidation,
                    Investigation = creatureEntity.Investigation,
                    Medicine = creatureEntity.Medicine,
                    Nature = creatureEntity.Nature,
                    Perception = creatureEntity.Perception,
                    Performance = creatureEntity.Performance,
                    Persuasion = creatureEntity.Persuasion,
                    Religion = creatureEntity.Religion,
                    SleightOfHand = creatureEntity.SleightOfHand,
                    Stealth = creatureEntity.Stealth,
                    Survival = creatureEntity.Survival,

                    SpeedUnit = speedUnit,
                    Walk = creatureEntity.Walk,
                    Crawl = creatureEntity.Crawl,
                    Hover = creatureEntity.Hover,
                    Fly = creatureEntity.Fly,
                    Burrow = creatureEntity.Burrow,
                    Climb = creatureEntity.Climb,
                    Swim = creatureEntity.Swim,

                    SenseUnit = senseUnit,
                    DarkvisionRange = creatureEntity.DarkvisionRange,
                    BlindsightRange = creatureEntity.BlindsightRange,
                    TremorsenseRange = creatureEntity.TremorsenseRange,
                    TruesightRange = creatureEntity.TruesightRange,

                    Languages = languages,
                    Traits = traits,
                    Actions = actions,
                    Legendaries = legendaries,
                    Biomes = biomes,

                    DamageImmunities = damageImmunities,
                    DamageResistances = damageResistances,
                    DamageVulnerabilities = damageVulnerabilities,
                    ConditionImmunities = conditionImmunities
                };
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to load creature {CreatureId}", id);
                return null;
            }
        }
    }
}
