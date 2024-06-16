using System.ComponentModel.DataAnnotations;

namespace BookShoppingUI.Models
{
    public class OrderStatus
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(35)]

        public string? StatusName { get; set; }
    }
}
