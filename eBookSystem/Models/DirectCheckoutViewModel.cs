using System.ComponentModel.DataAnnotations;

namespace eBookSystem.Models
{
    public class DirectCheckoutViewModel
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public Book Book { get; set; }

    }
}