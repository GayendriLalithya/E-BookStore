using System.Collections.Generic;

namespace eBookSystem.Models
{
    public class AdminDashboardViewModel
    {
        public decimal TotalSales { get; set; }
        public int TotalSalesCount { get; set; }
        public int TotalUsers { get; set; }
        public List<Book> MostPopularBooks { get; set; }
        public List<Order> RecentOrders { get; set; }
    }
}