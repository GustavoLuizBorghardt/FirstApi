using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace SeuProjeto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LongestGrowthController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public LongestGrowthController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("longestgrowth")]
        public async Task<IActionResult> GetLongestGrowth()
        {
            string apiKey = "LO3VVPS506E52Z3R";
            string[] symbols = new[] { "MSFT", "AAPL", "GOOGL", "AMZN", "TSLA" };

            string bestSymbol = "";
            int longestStreak = 0;

            foreach (var symbol in symbols)
            {
                string url = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&apikey={apiKey}";
                string response;

                try
                {
                    response = await _httpClient.GetStringAsync(url);
                }
                catch
                {
                    continue; // ignora se der erro de requisição
                }

                using var doc = JsonDocument.Parse(response);

                // Encontra a chave que contém "Time Series"
                var timeSeriesProp = doc.RootElement.EnumerateObject()
                    .FirstOrDefault(p => p.Name.Contains("Time Series"));

                if (timeSeriesProp.Value.ValueKind == JsonValueKind.Undefined)
                    continue;

                var timeSeries = timeSeriesProp.Value;

                // Pega os fechamentos em ordem decrescente (mais recente primeiro)
                var closingPrices = timeSeries.EnumerateObject()
                    .Select(d => decimal.TryParse(d.Value.GetProperty("4. close").GetString(), out var val) ? (decimal?)val : null)
                    .Where(val => val.HasValue)
                    .Select(val => val!.Value)
                    .ToArray();

                if (closingPrices.Length < 2)
                    continue;

                // Calcula o streak de alta contínua
                int streak = 0;
                for (int i = 0; i < closingPrices.Length - 1; i++)
                {
                    if (closingPrices[i] > closingPrices[i + 1])
                        streak++;
                    else
                        break; // quebra o streak na primeira queda
                }

                if (streak > longestStreak)
                {
                    longestStreak = streak;
                    bestSymbol = symbol;
                }
            }

            if (string.IsNullOrEmpty(bestSymbol))
            {
                return BadRequest(new { error = "Não foi possível obter dados válidos para os símbolos informados." });
            }

            return Ok(new
            {
                symbol = bestSymbol,
                consecutiveDaysUp = longestStreak
            });
        }
    }
}
