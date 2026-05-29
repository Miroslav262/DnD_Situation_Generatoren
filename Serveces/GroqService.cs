using System.Net.Http.Headers;
using System.Text.Json;
using dndsitgen.Models;

namespace dndsitgen.Services
{
    public class GroqService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public GroqService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _apiKey = config["Groq:ApiKey"]
                      ?? throw new Exception("Groq:ApiKey is missing");

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<string> AskAsync(string prompt)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
                "https://api.groq.com/openai/v1/chat/completions");

            request.Content = new StringContent(
                $$"""
        {
          "messages": [
            {
              "role": "user",
              "content": "{{prompt}}"
            }
          ],
          "model": "llama-3.3-70b-versatile"
        }
        """,
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await _http.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            var parsed = JsonSerializer.Deserialize<GroqResponse>(json);

            return parsed?.choices?.FirstOrDefault()?.message?.content
                   ?? "No content returned";
        }
    }
}
