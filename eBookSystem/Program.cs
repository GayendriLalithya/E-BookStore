using eBookSystem.Areas.Identity.Data;
using eBookSystem.Data;
using eBookSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceStack;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Ensure the connection strings are correctly configured
var connectionString = builder.Configuration.GetConnectionString("eBookSystemDBContextConnection") ?? throw new InvalidOperationException("Connection string 'eBookSystemDBContextConnection' not found.");

builder.Services.AddDbContext<eBookSystemDBContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<CustomerUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<eBookSystemDBContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Register the Database class for dependency injection
builder.Services.AddTransient<Database>();

// Configure Stripe settings
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));

// Add session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure Stripe
StripeConfiguration.ApiKey = builder.Configuration.GetSection("StripeSettings")["SecretKey"];

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Order",
    pattern: "{controller=Order}/{action=CreateCheckoutSession}");

app.MapControllerRoute(
    name: "order",
    pattern: "{controller=Order}/{action=Success}/{orderId?}");

app.MapControllerRoute(
    name: "DirectOrder",
    pattern: "{controller=DirectOrder}/{action=Checkout}/{bookId}/{quantity}");

app.MapControllerRoute(
    name: "DirectOrderProcessCheckout",
    pattern: "{controller=DirectOrder}/{action=ProcessCheckout}");

app.MapControllerRoute(
    name: "DirectOrderSuccess",
    pattern: "{controller=DirectOrder}/{action=Success}/{orderId?}");

app.MapControllerRoute(
    name: "Book",
    pattern: "{controller=Book}/{action=Create}/{id?}");

app.MapControllerRoute(
    name: "AdminLogin",
    pattern: "admin/adminLogin",
    defaults: new { controller = "Admin", action = "AdminLogin" });


app.MapControllerRoute(
    name: "AdminLogin",
    pattern: "admin/adminRegister",
    defaults: new { controller = "Admin", action = "AdminRegistration" });


app.MapControllerRoute(
    name: "manageBook",
    pattern: "admin/manageBook",
    defaults: new { controller = "Book", action = "ManageBook" });

app.MapControllerRoute(
    name: "AdminDashboard",
    pattern: "admin/adminDashboard",
    defaults: new { controller = "AdminDashboard", action = "Index" });

app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "manageOrder/manageOrders",
    pattern: "manageOrder",
    defaults: new { controller = "ManageOrder", action = "ManageOrders" });

app.MapControllerRoute(
    name: "SubmitRating",
    pattern: "{controller=ManageOrder}/{action=SubmitRating}");

app.MapControllerRoute(
    name: "ManageOrdersAdmin",
    pattern: "admin/manageOrders",
    defaults: new { controller = "ManageOrdersAdmin", action = "Index" });

app.MapControllerRoute(
    name: "ManageCustomers",
    pattern: "admin/manageCustomers",
    defaults: new { controller = "ManageCustomers", action = "Index" });

app.MapControllerRoute(
    name: "ViewOrderItems",
    pattern: "admin/manageOrders/ViewOrderItems/{orderId}",
    defaults: new { controller = "ManageOrdersAdmin", action = "ViewOrderItems" });

app.MapControllerRoute(
    name: "EditOrderStatus",
    pattern: "admin/manageOrders/EditOrderStatus",
    defaults: new { controller = "ManageOrdersAdmin", action = "EditOrderStatus" });

app.MapControllerRoute(
    name: "DeleteOrder",
    pattern: "admin/manageOrders/DeleteOrder",
    defaults: new { controller = "ManageOrdersAdmin", action = "DeleteOrder" });

app.MapControllerRoute(
    name: "CustomerLogin",
    pattern: "login",
    defaults: new { controller = "Account", action = "Login" });

app.MapControllerRoute(
    name: "BookDetails",
    pattern: "Book/Details/{id}",
    defaults: new { controller = "Book", action = "Details" });

app.MapControllerRoute(
    name: "SubmitReview",
    pattern: "{controller=Book}/{action=SubmitReview}");
app.MapControllerRoute(
    name: "Cart",
    pattern: "{controller=Cart}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "AddToCart",
    pattern: "{controller=Cart}/{action=AddToCart}/{bookId}/{quantity}");

app.MapControllerRoute(
    name: "RemoveFromCart",
    pattern: "{controller=Cart}/{action=RemoveFromCart}/{cartItemId}");

app.MapRazorPages();

app.Run();