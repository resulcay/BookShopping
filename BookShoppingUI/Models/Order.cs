using System.ComponentModel.DataAnnotations;

namespace BookShoppingUI.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? UserId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int OrderStatusId { get; set; }

        public OrderStatus? OrderStatus { get; set; }

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

        public bool IsDeleted { get; set; } = false;

        public bool IsPaid { get; set; } = false;

        public List<OrderDetail>? OrderDetails { get; set; }

    }
}
