using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using eBookSystem.Models; // Ensure this namespace is correct

public class PaymentController : Controller
{
    private readonly StripeSettings _stripeSettings;
    public string SessionId { get; set; }

    public PaymentController(IOptions<StripeSettings> stripeSettings)
    {
        _stripeSettings = stripeSettings.Value;
    }

    [HttpPost]
    public IActionResult CreateCheckoutSession(decimal amount)
    {
        var currency = "usd";
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
                        Currency = currency,
                        UnitAmount = (long)(amount * 100), // Amount in cents
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Order Payment"
                        }
                    },
                    Quantity = 1
                }
            },
            Mode = "payment",
            SuccessUrl = Url.Action("Success", "Payment", null, Request.Scheme),
            CancelUrl = Url.Action("Cancel", "Payment", null, Request.Scheme)
        };

        var service = new SessionService();
        var session = service.Create(options);
        SessionId = session.Id;

        return Redirect(session.Url);
    }

    public IActionResult Success()
    {
        return View();
    }

    public IActionResult Cancel()
    {
        return View();
    }
}
