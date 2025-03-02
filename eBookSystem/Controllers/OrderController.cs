using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using eBookSystem.Areas.Identity.Data;
using eBookSystem.Models;
using System.Threading.Tasks;
using System.Linq;

public class OrderController : Controller
{
    private readonly Database _database;
    private readonly UserManager<CustomerUser> _userManager;
    private readonly StripeSettings _stripeSettings;

    public OrderController(Database database, UserManager<CustomerUser> userManager, IOptions<StripeSettings> stripeSettings)
    {
        _database = database;
        _userManager = userManager;
        _stripeSettings = stripeSettings.Value;
    }

    [HttpGet]
    [HttpGet]
    public async Task<IActionResult> Checkout(decimal totalAmount)
    {
        var userId = User.Identity.Name;
        if (userId == null)
        {
            return Unauthorized();
        }

        var cart = _database.GetCartByUserId(userId);
        if (cart == null || !cart.CartItems.Any())
        {
            return BadRequest("Your cart is empty.");
        }

        var model = new CheckoutViewModel
        {
            TotalAmount = totalAmount,
            CartItems = cart.CartItems.ToList()
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

        // Create the order object
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

        // Insert order and order items
        _database.InsertOrder(order);
        _database.InsertOrderItem(order.Id, model.BookId, model.Quantity);

        // Update book quantity in stock
        _database.UpdateBookQuantity(model.BookId, -model.Quantity);

        // Create Stripe checkout session
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
            SuccessUrl = Url.Action("Success", "Order", new { orderId }, Request.Scheme),
            CancelUrl = Url.Action("Cancel", "Order", null, Request.Scheme)
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

        var cart = _database.GetCartByUserId(userId);
        if (cart != null)
        {
            // Insert order items and update book quantities
            foreach (var cartItem in cart.CartItems)
            {
                _database.InsertOrderItem(order.Id, cartItem.BookId, cartItem.Quantity);
                _database.UpdateBookQuantity(cartItem.BookId, -cartItem.Quantity); // Reduce book quantity
            }

            // Clear the cart
            _database.ClearCart(cart.Id);
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
