using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderingAPI.Data;
using OrderingAPI.Models;

namespace OrderingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly OrderingContext _context;

        public OrderController(OrderingContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.Items) // Inclure les articles associés
                .ToList();

            ViewData["Title"] = "My Orders"; // Titre de la vue
            return View(orders);
        }


        public static void Seed(OrderingContext context)
        {
            if (!context.Orders.Any())
            {
                var order = new Order
                {
                    UserName = "John Doe",
                    TotalPrice = 150.75m,
                    OrderDate = DateTime.Now,
                    Status = "On Shipping",
                    Address = "123 Main St, Example City",
                    PaymentMethod = "Credit Card",
                    Items = new List<OrderItem>
            {
                new OrderItem { ProductName = "Product A", Price = 50.25m, Quantity = 2, ImageUrl = "/images/trek-x1-black.jpg" },
                new OrderItem { ProductName = "Product B", Price = 50.25m, Quantity = 1, ImageUrl = "/images/cannondale-synapse-white.jpg" }
            }
                };

                context.Orders.Add(order);
                context.SaveChanges();
            }
        }

    }
}
