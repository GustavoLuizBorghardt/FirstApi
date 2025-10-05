using System.ComponentModel.DataAnnotations;

namespace PrimeiraApi.DTOs
{
    public class AddFavoriteStockDto
    {
        [Required]
        public string Ticker { get; set; } = string.Empty;
    }
}