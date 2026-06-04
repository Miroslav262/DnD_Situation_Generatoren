using System.Globalization;
using System.Text.Json;
using System.Threading.Tasks;
using dndsitgen.Models;

namespace dndsitgen.Serveces
{
    public class CreaturesService
    {
        private readonly HttpClient _httpClient;

        public CreaturesService(HttpClient httpClient) { 
            _httpClient = httpClient;
        }
        public async Task<int> getCount(String filter) {
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, "https://api.open5e.com/v2/creatures/?"+ filter);
            HttpResponseMessage httpResponse = await _httpClient.SendAsync(message);

            string json = await httpResponse.Content.ReadAsStringAsync();

            JsonDocument doc = JsonDocument.Parse(json);
            Console.WriteLine("JSON FROM API:\n" + json);

            return doc.RootElement.GetProperty("count").GetInt32();
        }
        public async Task<int> getCount()
        {
            return await getCount("");
        }
        
        public async Task<CreatureModel> getCreature(int id) {


            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, "https://api.open5e.com/v2/creatures/?limit=1&page="+id);
            HttpResponseMessage httpResponse = await _httpClient.SendAsync(message);

            string json = await httpResponse.Content.ReadAsStringAsync(); 
            JsonDocument jsonDocument = JsonDocument.Parse(json);

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return jsonDocument.RootElement.GetProperty("results").Deserialize<List<CreatureModel>>(options).First<CreatureModel>();
        }

        public async Task<CreatureModel> getRandomCreatureByCR(float cr) {

            try
            {
                Console.WriteLine("[DEBUG]: " + cr);
                string crStr = cr.ToString(CultureInfo.InvariantCulture);

                int count = await getCount("challenge_rating=" + crStr);
                int id = Random.Shared.Next() % count + 1;

                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, "https://api.open5e.com/v2/creatures/?limit=1&challenge_rating=" + crStr + "&page=" + id);
                HttpResponseMessage httpResponse = await _httpClient.SendAsync(message);

                string json = await httpResponse.Content.ReadAsStringAsync();
                JsonDocument jsonDocument = JsonDocument.Parse(json);

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return jsonDocument.RootElement.GetProperty("results").Deserialize<List<CreatureModel>>(options).First<CreatureModel>();
            }
            catch (Exception e)
            {
                Console.WriteLine("INNER EXCEPTION:\n" + e);
                throw;
            }




        }
        public async Task<CreatureModel> getCreatureByKey(string key)
        {

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, "https://api.open5e.com/v2/creatures/?key="+key);
            HttpResponseMessage httpResponse = await _httpClient.SendAsync(message);

            string json = await httpResponse.Content.ReadAsStringAsync();
            JsonDocument jsonDocument = JsonDocument.Parse(json);

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return jsonDocument.RootElement.GetProperty("results").Deserialize<List<CreatureModel>>(options).First<CreatureModel>();

        }
    }
}
