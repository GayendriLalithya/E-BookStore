using eBookSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;

public class CartController : Controller
{
    private readonly Database _database;

    public CartController(Database database)
    {
        _database = database;
    }

    public IActionResult Index()
    {
        // Get the logged-in user's email
        var userEmail = User.Identity.Name;
        if (string.IsNullOrEmpty(userEmail))
        {
            return Unauthorized(); // Handle unauthenticated users
        }

        // Fetch the cart using the user's email
        var cart = _database.GetCartByUserEmail_Single(userEmail);

        // If no cart exists, create a new empty cart for display
        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userEmail,
                CartItems = new List<CartItem>()
            };
        }

        return View(cart);
    }

    [HttpPost]
    public IActionResult AddToCart(int bookId, int quantity)
    {
        // Get the user's email or normalized user identifier
        string userEmail = User.Identity.Name;
        if (string.IsNullOrEmpty(userEmail))
        {
            return Unauthorized(); // Handle case where user is not logged in
        }

        // Retrieve the cart for the user
        var cart = _database.GetCartByUserEmail(userEmail); // Adjusted to use email
        if (cart == null)
        {
            // Create a new cart if it doesn't exist
            _database.CreateCart(userEmail);
            cart = _database.GetCartByUserEmail(userEmail);
            if (cart == null)
            {
                throw new Exception("Failed to create or retrieve cart.");
            }
        }

        // Add the item to the cart
        _database.AddToCart(cart.Id, bookId, quantity);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult UpdateQuantity(int cartItemId, int quantity)
    {
        if (quantity < 1)
        {
            _database.RemoveFromCart(cartItemId);
        }
        else
        {
            _database.UpdateCartItemQuantity(cartItemId, quantity);
        }
        return RedirectToAction("Index");
    }
    [HttpPost]
    public IActionResult RemoveFromCart(int cartItemId)
    {
        _database.RemoveFromCart(cartItemId);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult ClearCart()
    {
        var userId = User.Identity.Name;
        var cart = _database.GetCartByUserId(userId);
        if (cart != null)
        {
            _database.ClearCart(cart.Id);
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Checkout()
    {
        var userId = User.Identity.Name;
        var cart = _database.GetCartByUserId(userId);
        if (cart == null || !cart.CartItems.Any())
        {
            return BadRequest("Your cart is empty.");
        }

        var totalAmount = cart.CartItems.Sum(item => item.Book.Price * item.Quantity);
        return RedirectToAction("Checkout", "Order", new { totalAmount });
    }
}
