using InventoryManagement.Mvc.Data;
using InventoryManagement.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Mvc.Controllers
{
    public class ItemsController : Controller
    {

        private readonly IEnumerable<Item> _items;
        private readonly ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _items = new List<Item>
            {
                new Item { Id = 1, Name = "Laptop", Description = "Leistungsstarker Laptop", Price = 999.99m, Stock = 10 },
                new Item { Id = 2, Name = "Smartphone", Description = "Neuestes Modell", Price = 799.50m, Stock = 25 },
                new Item { Id = 3, Name = "Tablet", Description = "Handliches Tablet", Price = 399.00m, Stock = 15 }
            };
            _context = context;
        }
        public IActionResult Overview()
        {
            //return View(_items);
            var items = _context.Items.ToList();
            return View(items);
        }

        public IActionResult CreateEdit(int id)
        {

            Item item = null;


            //item = id > 0 ? _items.First(i => i.Id == id) : new Item();
            if (id > 0)
                item = _context.Items.First(i => i.Id == id);

            if (item == null)
                item = new Item();
            
            return View(item);

        }

        public IActionResult CreateEditItem(Item item)
        {

            if (!ModelState.IsValid)
            {
                return View(nameof(CreateEdit), item);
            }

            if (item.Id > 0)
            {
                _context.Items.Update(item);
            }
            else
            {
                _context.Items.Add(item);
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Overview));

        }

        public IActionResult DeleteItem(int id)
        {

            var item = _context.Items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Overview));

        }
    }
}
