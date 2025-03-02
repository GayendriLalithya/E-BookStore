using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using eBookSystem.Areas.Identity.Data;
using System.Linq;
using System.Threading.Tasks;

namespace eBookSystem.Controllers
{
    public class ManageCustomersController : Controller
    {
        private readonly UserManager<CustomerUser> _userManager;

        public ManageCustomersController(UserManager<CustomerUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var customers = _userManager.Users.ToList();
            return View(customers);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Error deleting user.");
                return View("Index", _userManager.Users.ToList());
            }

            return RedirectToAction("Index");
        }
    }
}