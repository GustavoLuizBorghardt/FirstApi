using System.ComponentModel.DataAnnotations;

namespace PrimeiraApi.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        public virtual ICollection<FavoriteStock> FavoriteStocks { get; set; } = new List<FavoriteStock>();
    }
}