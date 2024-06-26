﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShoppingUI.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string? BookName { get; set; }

        [Required, MaxLength(50)]
        public string? AuthorName { get; set; }

        public double Price { get; set; }

        public string? Image { get; set; }

        [Required]
        public int GenreId { get; set; }

        public Genre? Genre { get; set; }

        public List<OrderDetail>? OrderDetails { get; set; }

        public List<CartDetail>? CartDetails { get; set; }

        [NotMapped]
        public string? GenreName { get; set; }
    }
}
