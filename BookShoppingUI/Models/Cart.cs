using System.ComponentModel.DataAnnotations;

namespace BookShoppingUI.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? UserId { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
