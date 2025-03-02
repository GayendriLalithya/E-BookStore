using eBookSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.AspNetCore.Identity;

namespace eBookSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly Database _database;

        public AdminController(Database database)
        {
            _database = database;
        }

        // Action to display the admin login form
        public IActionResult AdminLogin()
        {
            return View();
        }

        public IActionResult AdminRegistration()
        {
            return View(); // Returns a view with a registration form
        }


        public IActionResult AdminDashboard()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("AdminLogin");
            }

            var totalSales = _database.GetTotalSales();
            var totalSalesCount = _database.GetTotalSalesCount();
            var totalUsers = _database.GetTotalUsers();
            var mostPopularBooks = _database.GetMostPopularBooks(4); // Change to get top 4 books
            var recentOrders = _database.GetRecentOrders(5);

            var model = new AdminDashboardViewModel
            {
                TotalSales = totalSales,
                TotalSalesCount = totalSalesCount,
                TotalUsers = totalUsers,
                MostPopularBooks = mostPopularBooks ?? new List<Book>(),
                RecentOrders = recentOrders ?? new List<Order>()
            };

            return View(model);
        }

        public IActionResult Book()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }

        public IActionResult ManageBook()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("AdminLogin");
            }

            var model = new BookViewModel
            {
                Book = new Book(),
                Books = _database.GetAllBooks()
            };
            return View(model);
        }

        public IActionResult UpdateBook(int id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("AdminLogin");
            }

            var book = _database.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // Action to handle the admin login request
        [HttpPost]
       [ValidateAntiForgeryToken] // Add anti-forgery token to prevent CSRF attacks
        public IActionResult AdminLogin(string username, string password)
        {
          // Perform authentication logic here
         if (username == "admin" && password == "123")
          {
               HttpContext.Session.SetString("IsAdminLoggedIn", "true");
               return RedirectToAction("AdminDashboard");
           }
           else
           {
               ViewBag.ErrorMessage = "Invalid username or password.";
               return View();
            }
        }

        private bool IsAdminLoggedIn()
        {
            return HttpContext.Session.GetString("IsAdminLoggedIn") == "true";
        }

        // POST: Admin/Register
        [HttpPost]
        public IActionResult Register(Admin admin)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Call the RegisterAdmin method from Database class
                    var result = _database.RegisterAdmin(admin);

                    if (result == "Registration successful!")
                    {
                        TempData["Success"] = result;
                        return RedirectToAction("Login", "Account"); // Redirect to login or other desired action
                    }
                    else
                    {
                        TempData["Error"] = result; // Display error message if registration fails
                    }
                }
                else
                {
                    TempData["Error"] = "Invalid input. Please fill in all required fields.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred: {ex.Message}";
            }

            return View(admin); // Return the view with the model in case of errors
        }
    }

}