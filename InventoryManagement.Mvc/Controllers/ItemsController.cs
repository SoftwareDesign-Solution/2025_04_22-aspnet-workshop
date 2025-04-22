using InventoryManagement.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Mvc.Controllers
{
    public class ItemsController : Controller
    {
        public IActionResult Overview()
        {

            var items = new List<Item>
            {
                new Item { Id = 1, Name = "Laptop", Description = "Leistungsstarker Laptop", Price = 999.99m, Stock = 10 },
                new Item { Id = 2, Name = "Smartphone", Description = "Neuestes Modell", Price = 799.50m, Stock = 25 },
                new Item { Id = 3, Name = "Tablet", Description = "Handliches Tablet", Price = 399.00m, Stock = 15 }
            };

            return View(items);
        }

        public IActionResult CreateEdit(int id)
        {
            return View();
        }

        public IActionResult DeleteItem(int id)
        {
            return RedirectToAction(nameof(Overview));
        }
    }
}
