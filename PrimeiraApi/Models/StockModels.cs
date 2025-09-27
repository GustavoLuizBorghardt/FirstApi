using System.Text.Json.Serialization;

namespace AlphaVantage.API.Models
{
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
    public class GlobalQuoteResponse
    {
        [JsonPropertyName("Global Quote")]
        public StockQuote? GlobalQuote { get; set; }
    }
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