using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using eBookSystem.Areas.Identity.Data;
using eBookSystem.Models;
using System.Linq;
using System.Threading.Tasks;

public class ManageOrderController : Controller
{
    private readonly Database _database;
    private readonly UserManager<CustomerUser> _userManager;

    public ManageOrderController(Database database, UserManager<CustomerUser> userManager)
    {
        _database = database;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> ManageOrders()
    {
        var userId = await GetUserIdAsync();
        if (userId == null)
        {
            return Unauthorized();
        }

        var orders = _database.GetOrdersByUserId(userId);
        foreach (var order in orders)
        {
            foreach (var item in order.OrderItems)
            {
                item.Rating = _database.GetUserRatingForBook(userId, order.Id, item.Book.Id);
            }
        }
        return View(orders);
    }

    [HttpPost]
    //public IActionResult SubmitRating([FromBody] RatingModel ratingModel)
    //{
    //    var userId = User.Identity.Name;
    //    _database.AddOrUpdateReview(userId, ratingModel.OrderId, ratingModel.BookId, ratingModel.Rating);
    //    return Ok(new { success = true });
    //}

    private async Task<string> GetUserIdAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        return user?.Id;
    }
}

public class RatingModel
{
    public int OrderId { get; set; }
    public int BookId { get; set; }
    public double Rating { get; set; }
}