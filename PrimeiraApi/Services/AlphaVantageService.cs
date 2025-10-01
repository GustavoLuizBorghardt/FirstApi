using AlphaVantage.API.Models;
using System.Globalization;

namespace PrimeiraApi.Services
{
    public class AlphaVantageService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public AlphaVantageService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["AlphaVantage:ApiKey"]
                      ?? throw new InvalidOperationException("API Key da Alpha Vantage não encontrada na configuração.");
        }

        public async Task<StockQuote?> GetRealTimeQuoteAsync(string symbol)
        {
            var requestUrl = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={_apiKey}";
            try
            {
                var response = await _httpClient.GetFromJsonAsync<GlobalQuoteResponse>(requestUrl);
                if (response?.GlobalQuote?.Symbol == null) return null;
                return response.GlobalQuote;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar cotação para {symbol}: {ex.Message}");
                return null;
            }
        }

        public async Task<TopMoversResponse?> GetTopMoversAsync()
        {
            var requestUrl = $"https://www.alphavantage.co/query?function=TOP_GAINERS_LOSERS&apikey={_apiKey}";
            try
            {
                return await _httpClient.GetFromJsonAsync<TopMoversResponse>(requestUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar Top Movers: {ex.Message}");
                return null;
            }
        }

        public async Task<DailyInfoResponse?> GetDailyInfoAsync(string symbol)
        {
            var requestUrl = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&outputsize=compact&apikey={_apiKey}";
            try
            {
                return await _httpClient.GetFromJsonAsync<DailyInfoResponse>(requestUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar dados diários para {symbol}: {ex.Message}");
                return null;
            }
        }

        public async Task<(string Symbol, int Streak)> GetLongestGrowthStreakAsync(string symbol)
        {
            var dailyInfo = await GetDailyInfoAsync(symbol);
            if (dailyInfo?.TimeSeries == null || dailyInfo.TimeSeries.Count < 2)
            {
                return (symbol, 0);
            }

            var sortedData = dailyInfo.TimeSeries
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => decimal.Parse(kvp.Value.Close, CultureInfo.InvariantCulture))
                .ToList();

            int longestStreak = 0;
            int currentStreak = 0;

            for (int i = 1; i < sortedData.Count; i++)
            {
                if (sortedData[i] > sortedData[i - 1])
                {
                    currentStreak++;
                }
                else
                {
                    if (currentStreak > longestStreak) longestStreak = currentStreak;
                    currentStreak = 0;
                }
            }
            if (currentStreak > longestStreak) longestStreak = currentStreak;

            return (symbol, longestStreak);
        }
    }
}