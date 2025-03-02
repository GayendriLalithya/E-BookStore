namespace eBookSystem.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public double Rating { get; set; } // Add this line
        public Book Book { get; set; }
    }
}