using System.ComponentModel.DataAnnotations;

namespace BookShoppingUI.Models.DTOs
{
    public class CheckOutModel
    {
        [Required]
        [StringLength(200)]
        public string? Address { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(40)]
        public string? Email { get; set; }

        [Required]
        [StringLength(30)]
        public string? PaymentMethod { get; set; }

        [Required]
        [StringLength(20)]
        public string? Phone { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
