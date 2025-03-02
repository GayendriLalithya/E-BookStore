using Microsoft.AspNetCore.Mvc;

namespace eBookSystem.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
