using System.ComponentModel.DataAnnotations;

namespace BookShoppingUI.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string? GenreName { get; set; }

        public List<Book>? Books { get; set; }
    }
}
