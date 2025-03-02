using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using eBookSystem.Areas.Identity.Data;
using eBookSystem.Models;
using System.Threading.Tasks;

public class DirectOrderController : Controller
{
    private readonly Database _database;
    private readonly UserManager<CustomerUser> _userManager;
    private readonly StripeSettings _stripeSettings;

    public DirectOrderController(Database database, UserManager<CustomerUser> userManager, IOptions<StripeSettings> stripeSettings)
    {
        _database = database;
        _userManager = userManager;
        _stripeSettings = stripeSettings.Value;
    }

    [HttpGet]
    public async Task<IActionResult> Checkout(int bookId, int quantity)
    {
        var userId = await GetUserIdAsync();
        if (userId == null)
        {
            return Unauthorized();
        }

        var book = _database.GetBookById(bookId);
        if (book == null)
        {
            return NotFound();
        }

        decimal totalAmount = book.Price.GetValueOrDefault() * quantity;

        var model = new DirectCheckoutViewModel
        {
            BookId = bookId,
            Quantity = quantity,
            TotalAmount = totalAmount,
            Book = book
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ProcessCheckout(DirectCheckoutViewModel model)
    {
       
            var userId = await GetUserIdAsync();
            if (userId == null)
            {
                return Unauthorized();
            }

            var order = new Order
            {
                UserId = userId,
                FullName = model.FullName,
                Address = model.Address,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                Country = model.Country,
                TotalAmount = model.TotalAmount,
                OrderDate = DateTime.Now,
                Status = "Pending"
            };
            
            _database.InsertOrder(order);
            _database.InsertOrderItem(order.Id, model.BookId, model.Quantity);
            _database.UpdateBookQuantity(model.BookId, -model.Quantity);

            var sessionUrl = CreateCheckoutSession(model.TotalAmount, order.Id);

            return Redirect(sessionUrl);
      
    }

    private async Task<string> GetUserIdAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        return user?.Id;
    }

    private string CreateCheckoutSession(decimal amount, int orderId)
    {
        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "LKR",
                        UnitAmount = (long)(amount * 100), // Amount in cents
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Book Purchase"
                        }
                    },
                    Quantity = 1
                }
            },
            Mode = "payment",
            SuccessUrl = Url.Action("Success", "DirectOrder", new { orderId }, Request.Scheme),
            CancelUrl = Url.Action("Cancel", "DirectOrder", null, Request.Scheme)
        };

        var service = new SessionService();
        var session = service.Create(options);
        return session.Url;
    }

    public async Task<IActionResult> Success(int orderId)
    {
        var userId = await GetUserIdAsync();
        if (userId == null)
        {
            return Unauthorized();
        }

        var order = _database.GetOrderById(orderId);
        if (order == null || order.UserId != userId)
        {
            return NotFound();
        }

        order.Status = "Confirmed";
        _database.UpdateOrderStatus(order);

        return RedirectToAction("OrderDetail", new { orderId = order.Id });
    }

    public IActionResult Cancel()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> OrderDetail(int orderId)
    {
        var userId = await GetUserIdAsync();
        if (userId == null)
        {
            return Unauthorized();
        }

        var order = _database.GetOrderById(orderId);
        if (order == null || order.UserId != userId)
        {
            return NotFound();
        }

        var orderItems = _database.GetOrderItems(orderId);
        var model = new OrderDetailViewModel
        {
            Order = order,
            OrderItems = orderItems
        };

        return View(model);
    }
}