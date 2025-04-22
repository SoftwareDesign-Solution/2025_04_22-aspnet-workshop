using InventoryManagement.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Mvc.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Overview()
        {
            var orders = new List<Order>
            {
                new Order { Id = 1, OrderDate = DateTime.Now }
            };

            return View(orders);
        }

        public IActionResult OrderDetails(int id)
        {
            var order = new Order
            {
                Id = id,
                OrderDate = DateTime.Now,
                Comments = "Testkommentar",
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Id = 1, ItemId = 1, Item = new Item { Id = 1, Name = "Laptop", Description = "Leistungsstarker Laptop", Price = 999.99m, Stock = 10 }, Quantity = 2 },
                    new OrderItem { Id = 2, ItemId = 2, Item = new Item { Id = 2, Name = "Smartphone", Description = "Neuestes Modell", Price = 799.50m, Stock = 25 }, Quantity = 1 }
                }
            };
            return View(order);
        }

        public IActionResult CreateEdit(int id)
        {
            return View();
        }
        public IActionResult DeleteOrder(int id)
        {
            return RedirectToAction(nameof(Overview));
        }
    }
}
