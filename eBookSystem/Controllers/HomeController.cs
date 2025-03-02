using eBookSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;

namespace eBookSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Database _database;

        public HomeController(ILogger<HomeController> logger, Database database)
        {
            _logger = logger;
            _database = database;
        }

        public IActionResult BookStore()
        {
            List<Book> books = _database.GetAllBooks();
            return View(books);
        }

        public IActionResult Index()
        {
            List<Book> books = _database.GetAllBooks();
            return View(books);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
