using System.ComponentModel.DataAnnotations;

namespace PrimeiraApi.Data.Entities
{
    public class FavoriteStock
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Ticker { get; set; } = string.Empty;

        public int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}