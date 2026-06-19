using Microsoft.Extensions.Logging;
using Npgsql;
using dndsitgen.Models;

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
            await using NpgsqlConnection connection = new NpgsqlConnection(con_str);

            try
            {
                connection.Open();
                const string query = """
                     SELECT * FROM creature WHERE id = @id 
                    """;

                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("id", id);

                await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

                if (!await reader.ReadAsync())
                    return null;
                return MapCreature(reader);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to open connection");
                return null;
            }
        }
        private static CreatureModel MapCreature(NpgsqlDataReader reader)
        {
            return new CreatureModel
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                ImageUrl = reader["image_url"] as string,
                Description = reader["description"] as string,
                SizeId = reader.GetInt32(reader.GetOrdinal("size_id")),
                TypeId = reader.GetInt32(reader.GetOrdinal("type_id")),
                AlignmentId = reader.GetInt32(reader.GetOrdinal("alignment_id")),
                Ac = reader.GetInt32(reader.GetOrdinal("ac")),
                HpDefault = reader.IsDBNull(reader.GetOrdinal("hp_default")) ? null : reader.GetInt32(reader.GetOrdinal("hp_default")),
                HpDice = reader.GetInt32(reader.GetOrdinal("hp_dice")),
                HpDiceCount = reader.GetInt32(reader.GetOrdinal("hp_dice_count")),
                HpAddition = reader.GetInt32(reader.GetOrdinal("hp_addition")),
                Passive = reader.GetInt32(reader.GetOrdinal("passive")),
                CrId = reader.GetInt32(reader.GetOrdinal("cr_id")),
                SourceId = reader.IsDBNull(reader.GetOrdinal("source_id")) ? null : reader.GetInt32(reader.GetOrdinal("source_id")),
                Strength = reader.GetInt32(reader.GetOrdinal("strength")),
                Dexterity = reader.GetInt32(reader.GetOrdinal("dexterity")),
                Constitution = reader.GetInt32(reader.GetOrdinal("constitution")),
                Intelligence = reader.GetInt32(reader.GetOrdinal("intelligence")),
                Wisdom = reader.GetInt32(reader.GetOrdinal("wisdom")),
                Charisma = reader.GetInt32(reader.GetOrdinal("charisma")),
                SavingStrength = reader.GetInt32(reader.GetOrdinal("saving_strength")),
                SavingDexterity = reader.GetInt32(reader.GetOrdinal("saving_dexterity")),
                SavingConstitution = reader.GetInt32(reader.GetOrdinal("saving_constitution")),
                SavingIntelligence = reader.GetInt32(reader.GetOrdinal("saving_intelligence")),
                SavingWisdom = reader.GetInt32(reader.GetOrdinal("saving_wisdom")),
                SavingCharisma = reader.GetInt32(reader.GetOrdinal("saving_charisma")),
                Acrobatics = reader.GetInt32(reader.GetOrdinal("acrobatics")),
                AnimalHandling = reader.GetInt32(reader.GetOrdinal("animal_handling")),
                Arcana = reader.GetInt32(reader.GetOrdinal("arcana")),
                Athletics = reader.GetInt32(reader.GetOrdinal("athletics")),
                Deception = reader.GetInt32(reader.GetOrdinal("deception")),
                History = reader.GetInt32(reader.GetOrdinal("history")),
                Insight = reader.GetInt32(reader.GetOrdinal("insight")),
                Intimidation = reader.GetInt32(reader.GetOrdinal("intimidation")),
                Investigation = reader.GetInt32(reader.GetOrdinal("investigation")),
                Medicine = reader.GetInt32(reader.GetOrdinal("medicine")),
                Nature = reader.GetInt32(reader.GetOrdinal("nature")),
                Perception = reader.GetInt32(reader.GetOrdinal("perception")),
                Performance = reader.GetInt32(reader.GetOrdinal("performance")),
                Persuasion = reader.GetInt32(reader.GetOrdinal("persuasion")),
                Religion = reader.GetInt32(reader.GetOrdinal("religion")),
                SleightOfHand = reader.GetInt32(reader.GetOrdinal("sleight_of_hand")),
                Stealth = reader.GetInt32(reader.GetOrdinal("stealth")),
                Survival = reader.GetInt32(reader.GetOrdinal("survival")),
                SpeedUnitId = reader.GetInt32(reader.GetOrdinal("speed_unit_id")),
                Walk = reader["walk"] as int?,
                Crawl = reader["crawl"] as int?,
                Hover = reader["hover"] as int?,
                Fly = reader["fly"] as int?,
                Burrow = reader["burrow"] as int?,
                Climb = reader["climb"] as int?,
                Swim = reader["swim"] as int?,
                CreatureSensesUnitId = reader.GetInt32(reader.GetOrdinal("creature_senses_unit_id")),
                DarkvisionRange = reader["darkvision_range"] as int?,
                BlindsightRange = reader["blindsight_range"] as int?,
                TremorsenseRange = reader["tremorsense_range"] as int?,
                TruesightRange = reader["truesight_range"] as int?
            };
        }
    }
}
