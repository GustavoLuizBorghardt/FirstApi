using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace StockApp.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty(SupportsGet = true)]
        public string? CompanyName { get; set; }

        public StockQuote? Quote { get; private set; }
        public string? ErrorMessage { get; private set; }

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            if (string.IsNullOrWhiteSpace(CompanyName))
            {
                return;
            }

            var httpClient = _httpClientFactory.CreateClient("StockApiClient");

            try
            {
                Quote = await httpClient.GetFromJsonAsync<StockQuote>($"/api/stocks/quote-by-name/{CompanyName}");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                Console.WriteLine($"--- ERRO AO CHAMAR A API: {ex.ToString()} ---");
            }
        }
    }

    // --- A CORREÇÃO FINAL ESTÁ AQUI ---
    // Esta classe agora é uma cópia exata do modelo da nossa API principal,
    // incluindo os atributos JsonPropertyName com os nomes originais da Alpha Vantage.
    public class StockQuote
    {
        [JsonPropertyName("01. symbol")]
        public string Symbol { get; set; } = string.Empty;

        [JsonPropertyName("05. price")]
        public string Price { get; set; } = string.Empty;

        [JsonPropertyName("07. latest trading day")]
        public string LatestTradingDay { get; set; } = string.Empty;

        [JsonPropertyName("10. change percent")]
        public string ChangePercent { get; set; } = string.Empty;
    }
}

