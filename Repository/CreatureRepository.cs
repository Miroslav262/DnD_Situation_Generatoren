using Microsoft.Extensions.Logging;
using Npgsql;

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
    }
}
