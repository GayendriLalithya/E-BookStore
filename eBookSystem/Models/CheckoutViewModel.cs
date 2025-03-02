using System.Collections.Generic;
namespace eBookSystem.Models
{
    public class CheckoutViewModel
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>(); // Add this property
    }
}
