using Microsoft.AspNetCore.Mvc;
using eBookSystem.Models;

namespace eBookSystem.Controllers
{
    public class ManageOrdersAdminController : Controller
    {
        private readonly Database _database;

        public ManageOrdersAdminController(Database database)
        {
            _database = database;
        }

        public IActionResult Index()
        {
            var orders = _database.GetAllOrders();
            return View(orders);
        }

        [HttpGet]
        public IActionResult ViewOrderItems(int orderId)
        {
            var orderItems = _database.GetOrderItems(orderId);
            return PartialView("_OrderItemsPartial", orderItems);
        }

        [HttpPost]
        public IActionResult EditOrderStatus(int id, string status)
        {
            var order = _database.GetOrderById(id);
            if (order != null)
            {
                order.Status = status;
                _database.UpdateOrderStatus(order);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteOrder(int id)
        {
            _database.DeleteOrder(id);
            return RedirectToAction("Index");
        }
    }
}