using InventoryManagement.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Mvc.Controllers
{
    public class ItemsController : Controller
    {

        private readonly IEnumerable<Item> _items;

        public ItemsController()
        {
            _items = new List<Item>
            {
                new Item { Id = 1, Name = "Laptop", Description = "Leistungsstarker Laptop", Price = 999.99m, Stock = 10 },
                new Item { Id = 2, Name = "Smartphone", Description = "Neuestes Modell", Price = 799.50m, Stock = 25 },
                new Item { Id = 3, Name = "Tablet", Description = "Handliches Tablet", Price = 399.00m, Stock = 15 }
            };
        }
        public IActionResult Overview()
        {
            return View(_items);
        }

        public IActionResult CreateEdit(int id)
        {

            Item item = null;


            item = id > 0 ? _items.First(i => i.Id == id) : new Item();
            
            return View(item);

        }

        public IActionResult CreateEditItem(Item item)
        {

            if (!ModelState.IsValid)
            {
                return View(nameof(CreateEdit), item);
            }

            return RedirectToAction(nameof(Overview));
        }

        public IActionResult DeleteItem(int id)
        {
            return RedirectToAction(nameof(Overview));
        }
    }
}
