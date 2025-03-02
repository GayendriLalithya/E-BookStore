using System;

namespace eBookSystem.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string UserId { get; set; }
        public string ReviewText { get; set; }
        public DateTime Timestamp { get; set; }
        public float Rating { get; set; }
    }
}