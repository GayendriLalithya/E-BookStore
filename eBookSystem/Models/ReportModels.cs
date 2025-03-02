namespace eBookSystem.Models
{
    public class SalesReport
    {
        public DateTime Date { get; set; }
        public decimal TotalSales { get; set; }
        public int NumberOfBooksSold { get; set; }
    }

    public class SalesByBookReport
    {
        public string BookTitle { get; set; }
        public int UnitsSold { get; set; }
        public decimal RevenueGenerated { get; set; }
    }

    public class SalesByAuthorReport
    {
        public string AuthorName { get; set; }
        public int UnitsSold { get; set; }
        public decimal RevenueGenerated { get; set; }
    }

    public class SalesByCategoryReport
    {
        public string CategoryName { get; set; }
        public int UnitsSold { get; set; }
        public decimal RevenueGenerated { get; set; }
    }

    public class SalesByCustomerReport
    {
        public string CustomerName { get; set; }
        public int UnitsBought { get; set; }
        public decimal TotalSpent { get; set; }
    }

    public class InventoryStatusReport
    {
        public string BookTitle { get; set; }
        public int Available { get; set; }
        public int Reserved { get; set; }
        public int Sold { get; set; }
    }

    public class OutOfStockReport
    {
        public string BookTitle { get; set; }
        public int Available { get; set; }
    }

    public class LowStockAlertReport
    {
        public string BookTitle { get; set; }
        public int Available { get; set; }
    }

    public class ReportRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}