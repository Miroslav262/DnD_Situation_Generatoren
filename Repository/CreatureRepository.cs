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
        private readonly float SIMILARITY_CONST = 0.3f;

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

        public async Task<CreatureModel?> getByIdAsync(long id)
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

        public async Task<CreatureModel[]?> getFilteredCreaturesAsync(CreatureFilter filter)
        {
            await using var connection = new NpgsqlConnection(con_str);
            await connection.OpenAsync();

            var sql = """
                SELECT "id"
                FROM "creature"
                WHERE 1 = 1
            """;

            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                sql += " AND similarity(\"name\", @name) > @sim";
                parameters.Add("name", filter.Name);
                parameters.Add("sim", SIMILARITY_CONST);
            }

            if (!string.IsNullOrWhiteSpace(filter.Cr))
            {
                sql += """
                    AND "cr_id" = (
                        SELECT "id" FROM "creature_cr"
                        WHERE "cr" = @cr
                    )
                """;
                parameters.Add("cr", filter.Cr);
            }

            if (!string.IsNullOrWhiteSpace(filter.Alignment))
            {
                sql += """
                    AND "alignment_id" = (
                        SELECT "id" FROM "creature_alignment"
                        WHERE "name" = @alignment
                    )
                """;
                parameters.Add("alignment", filter.Alignment);
            }

            if (filter.Ac.HasValue)
            {
                sql += " AND \"ac\" = @ac";
                parameters.Add("ac", filter.Ac.Value);
            }

            if (filter.Passive.HasValue)
            {
                sql += " AND \"passive\" = @passive";
                parameters.Add("passive", filter.Passive.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Size))
            {
                sql += """
                    AND "size_id" = (
                        SELECT "id" FROM "creature_size"
                        WHERE "name" = @size OR "letter" = @size
                    )
                """;
                parameters.Add("size", filter.Size);
            }

            if (!string.IsNullOrWhiteSpace(filter.Type))
            {
                sql += """
                    AND "type_id" = (
                        SELECT "id" FROM "creature_type"
                        WHERE "name" = @type
                    )
                """;
                parameters.Add("type", filter.Type);
            }

            if (!string.IsNullOrWhiteSpace(filter.Source))
            {
                sql += """
                    AND "source_id" = (
                        SELECT "id" FROM "creature_source"
                        WHERE "name" = @source
                    )
                """;
                parameters.Add("source", filter.Source);
            }

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                sql += " ORDER BY similarity(\"name\", @name) DESC";
            }

            sql += ";";

            int[]? ids = (await connection.QueryAsync<int>(sql, parameters)).ToArray();

            var tasks = ids.Select(id => getByIdAsync(id)).ToArray();
            var creatures = await Task.WhenAll(tasks);

            return creatures.Where(c => c != null).ToArray()!;
        }

        private async Task<long?> getRandomId() {
            NpgsqlConnection connection = new NpgsqlConnection(con_str);
            try
            {
                await connection.OpenAsync();

                long id = await connection.ExecuteScalarAsync<long>(
                    @"SELECT ""id""
                  FROM ""creature""
                  OFFSET floor(random() * (SELECT COUNT(*) FROM ""creature""))
                  LIMIT 1;");
                return id;
            }
            catch {
                return null;
            }
        }

        public async Task<CreatureModel?> getRandomCreatureAsync() {
            long? id = await getRandomId();
            if (id == null) return null;

            return await getByIdAsync((int)id);
        }
           




        public async Task<bool> AddCreatureToScene(int sceneId, int creatureId, int count)
        {
            await using var connection = new NpgsqlConnection(con_str);
            await connection.OpenAsync();

            int affected = await connection.ExecuteAsync(
                """
                insert into "battle_scene_creature_ratio" (battle_scene_id, creature_id, count)
                values (@sceneId, @creatureId, @count)
                on conflict (battle_scene_id, creature_id)
                do update set count = excluded.count;
                """,
                new { sceneId, creatureId, count }
            );

            return affected > 0;
        }

        public async Task<bool> UpdateCreatureCount(int sceneId, int creatureId, int count)
        {
            await using var connection = new NpgsqlConnection(con_str);
            await connection.OpenAsync();

            int affected = await connection.ExecuteAsync(
                """
            update "battle_scene_creature_ratio"
            set count = @count
            where battle_scene_id = @sceneId and creature_id = @creatureId;
            """,
                new { sceneId, creatureId, count }
            );

            return affected > 0;
        }

        public async Task<bool> ChangeCreature(int sceneId, int creatureId, int newCreatureId)
        {
            await using var connection = new NpgsqlConnection(con_str);
            await connection.OpenAsync();

            int affected = await connection.ExecuteAsync(
                """
            update "battle_scene_creature_ratio"
            set creatureId = @newCreatureId
            where battle_scene_id = @sceneId and creature_id = @creatureId;
            """,
                new { sceneId, creatureId, newCreatureId }
            );

            return affected > 0;
        }

        public async Task<bool> DeleteCreatureFromScene(int sceneId, int creatureId)
        {
            await using var connection = new NpgsqlConnection(con_str);
            await connection.OpenAsync();

            int affected = await connection.ExecuteAsync(
                    """
            delete from "battle_scene_creature_ratio"
            where battle_scene_id = @sceneId and creature_id = @creatureId;
            """,
                new { sceneId, creatureId }
            );

            return affected > 0;
        }
        public async Task<CreatureCollectionRatio[]> GetCreaturesInScene(int sceneId)
        {
            await using var connection = new NpgsqlConnection(con_str);
            await connection.OpenAsync();

            var result = await connection.QueryAsync<CreatureCollectionRatio>(
                    """
            select creature_id as CreatureId, count
            from "battle_scene_creature_ratio"
            where battle_scene_id = @sceneId;
            """,
                new { sceneId }
            );

            return result.ToArray();
        }


    }
}
