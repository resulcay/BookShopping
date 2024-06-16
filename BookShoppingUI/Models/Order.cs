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

        public bool IsDeleted { get; set; } = false;

        public List<OrderDetail>? OrderDetails { get; set; }

    }
}
