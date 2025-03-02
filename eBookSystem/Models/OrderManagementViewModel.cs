using System.Collections.Generic;
namespace eBookSystem.Models
{
    

    public class OrderManagementViewModel
    {
        public List<Order> Orders { get; set; }
        public List<string> StatusOptions { get; set; }
        public string SelectedStatus { get; set; }
    }
}
