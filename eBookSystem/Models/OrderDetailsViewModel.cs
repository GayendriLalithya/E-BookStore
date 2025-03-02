using System.Collections.Generic;

namespace eBookSystem.Models
{
    public class OrderDetailViewModel
    {
        public Order Order { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
