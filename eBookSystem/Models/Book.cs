using System;
using System.Collections.Generic;

namespace eBookSystem.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Publisher { get; set; }
        public int PublishedYear { get; set; }
        public string Edition { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public string Condition { get; set; }
        public string Description { get; set; }
        public byte[] CoverImage { get; set; } // Ensure this is not marked as required
        public double? AverageRating { get; set; } // Ensure this is not marked as required
        public List<Review> Reviews { get; set; } = new List<Review>(); // Add Reviews property
    }
}