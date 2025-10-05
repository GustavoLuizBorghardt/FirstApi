namespace PrimeiraApi.DTOs
{
    // DTO para representar uma ação favorita na resposta
    public class FavoriteStockResponseDto
    {
        public int Id { get; set; }
        public string Ticker { get; set; } = string.Empty;
        public int UserId { get; set; }
    }

    // DTO para representar um usuário na resposta
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // A lista de favoritos usará o DTO de favorito, quebrando o ciclo
        public List<FavoriteStockResponseDto> FavoriteStocks { get; set; } = new();
    }

    // NOVO DTO: Para a resposta do endpoint que busca as cotações dos favoritos
    public class FavoriteStockQuoteDto
    {
        public string Ticker { get; set; } = string.Empty;
        public string Price { get; set; } = "N/A"; // "N/A" caso a API falhe para esta ação
        public string ChangePercent { get; set; } = "N/A";
    }
}