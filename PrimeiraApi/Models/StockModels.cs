using System.Text.Json.Serialization;

namespace PrimeiraApi.Models // <-- GARANTIR QUE ESTÁ CORRETO
{
    // Modelo para o endpoint TOP_GAINERS_LOSERS
    public class TopMoversResponse
    {
        [JsonPropertyName("top_gainers")]
        public List<StockMover> TopGainers { get; set; } = new();

        [JsonPropertyName("top_losers")]
        public List<StockMover> TopLosers { get; set; } = new();
    }

    public class StockMover
    {
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; } = string.Empty;

        [JsonPropertyName("price")]
        public string Price { get; set; } = string.Empty;

        [JsonPropertyName("change_percentage")]
        public string ChangePercentage { get; set; } = string.Empty;
    }

    // Modelo para o endpoint TIME_SERIES_DAILY
    public class DailyInfoResponse
    {
        [JsonPropertyName("Meta Data")]
        public DailyMetaData? MetaData { get; set; }

        [JsonPropertyName("Time Series (Daily)")]
        public Dictionary<string, DailyDataPoint>? TimeSeries { get; set; }
    }

    public class DailyMetaData
    {
        [JsonPropertyName("2. Symbol")]
        public string Symbol { get; set; } = string.Empty;
    }

    public class DailyDataPoint
    {
        [JsonPropertyName("4. close")]
        public string Close { get; set; } = string.Empty;
    }

    // Modelo para a resposta completa do endpoint GLOBAL_QUOTE
    public class GlobalQuoteResponse
    { 
        [JsonPropertyName("Global Quote")]
        public StockQuote? GlobalQuote { get; set; }
    }

    // Modelo para os detalhes da cotação de uma ação
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

