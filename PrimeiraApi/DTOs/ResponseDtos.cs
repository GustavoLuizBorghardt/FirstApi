namespace PrimeiraApi.DTOs
{
    public class FavoriteStockResponseDto
    {
        public int Id { get; set; }
        public string Ticker { get; set; } = string.Empty;
        public int UserId { get; set; }
    }

    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public List<FavoriteStockResponseDto> FavoriteStocks { get; set; } = new();
    }

    public class FavoriteStockQuoteDto
    {
        public string Ticker { get; set; } = string.Empty;
        public string Price { get; set; } = "N/A"; 
        public string ChangePercent { get; set; } = "N/A";
    }
}