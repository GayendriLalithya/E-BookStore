using Microsoft.AspNetCore.Mvc;
using eBookSystem.Models;
using System;
using System.Collections.Generic;

namespace eBookSystem.Controllers
{
    public class ReportsController : Controller
    {
        private readonly Database _database;

        public ReportsController(Database database)
        {
            _database = database;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GenerateOverallSalesReport([FromBody] DateRangeModel model)
        {
            var reports = _database.GetOverallSalesReport(model.FromDate, model.ToDate);
            return Json(reports);
        }

        [HttpPost]
        public JsonResult GenerateSalesByBookReport([FromBody] DateRangeModel model)
        {
            var reports = _database.GetSalesByBookReport(model.FromDate, model.ToDate);
            return Json(reports);
        }

        [HttpPost]
        public JsonResult GenerateSalesByAuthorReport([FromBody] DateRangeModel model)
        {
            var reports = _database.GetSalesByAuthorReport(model.FromDate, model.ToDate);
            return Json(reports);
        }

        [HttpPost]
        public JsonResult GenerateSalesByCategoryReport([FromBody] DateRangeModel model)
        {
            var reports = _database.GetSalesByCategoryReport(model.FromDate, model.ToDate);
            return Json(reports);
        }

        [HttpPost]
        public JsonResult GenerateSalesByCustomerReport([FromBody] DateRangeModel model)
        {
            var reports = _database.GetSalesByCustomerReport(model.FromDate, model.ToDate);
            return Json(reports);
        }

        [HttpPost]
        public JsonResult GenerateInventoryStatusReport()
        {
            var reports = _database.GetInventoryStatusReport();
            return Json(reports);
        }

        [HttpPost]
        public JsonResult GenerateOutOfStockReport()
        {
            var reports = _database.GetOutOfStockReport();
            return Json(reports);
        }

        [HttpPost]
        public JsonResult GenerateLowStockAlertReport()
        {
            var reports = _database.GetLowStockAlertReport();
            return Json(reports);
        }
    }

    public class DateRangeModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}