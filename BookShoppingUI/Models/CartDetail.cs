using System.ComponentModel.DataAnnotations;

namespace BookShoppingUI.Models
{
    public class CartDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CartId { get; set; }

        public Cart? Cart { get; set; }

        [Required]
        public int BookId { get; set; }

        public Book? Book { get; set; }

        public int Quantity { get; set; }
    }
}
