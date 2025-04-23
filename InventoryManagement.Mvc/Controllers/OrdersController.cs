using System.Reflection.Metadata.Ecma335;
using InventoryManagement.Mvc.Data;
using InventoryManagement.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Mvc.Controllers
{
    public class OrdersController : Controller
    {

        //private readonly IEnumerable<Item> _items;
        //private readonly IEnumerable<Order> _orders;
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            //_items = new List<Item>
            //{
            //    new Item { Id = 1, Name = "Laptop", Description = "Leistungsstarker Laptop", Price = 999.99m, Stock = 10 },
            //    new Item { Id = 2, Name = "Smartphone", Description = "Neuestes Modell", Price = 799.50m, Stock = 25 },
            //    new Item { Id = 3, Name = "Tablet", Description = "Handliches Tablet", Price = 399.00m, Stock = 15 }
            //};
            //_orders = new List<Order>
            //{
            //    new Order
            //    {
            //        Id = 1,
            //        OrderDate = DateTime.Now,
            //        Comment = "Testkommentar",
            //        OrderItems = new List<OrderItem>
            //        {
            //            new OrderItem { Id = 1, ItemId = 1, Item = new Item { Id = 1, Name = "Laptop", Description = "Leistungsstarker Laptop", Price = 999.99m, Stock = 10 }, Quantity = 2 },
            //            new OrderItem { Id = 2, ItemId = 2, Item = new Item { Id = 2, Name = "Smartphone", Description = "Neuestes Modell", Price = 799.50m, Stock = 25 }, Quantity = 1 }
            //        }
            //    }
            //};
            _context = context;
        }

        public IActionResult Overview()
        {

            // SELECT * FROM Orders
            //var orders = _context.Orders.ToList();


            var query = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item);
                
            var sql = query.ToQueryString();

            var orders = query.ToList();

            return View(orders);
        }

        public IActionResult OrderDetails(int id)
        {

            Order order = null;

            if (id > 0)
            {
                order = _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Item)
                    .First(o => o.Id == id);
            }

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
            
        }

        public IActionResult CreateEdit(int id)
        {

            OrderDto order = null;


            var availableItems = _context.Items
                .Where(i => i.Stock > 0)
                .ToList();

            ViewBag.AvailableItems = availableItems;

            //if (id > 0)
            //{
            //    order = _orders.First(o => o.Id == id);
            //}

            if (order != null) return View(order);

            order = new OrderDto();

            foreach (var item in availableItems)
            {
                order.OrderItems.Add(new OrderItem { ItemId = item.Id, Item = item });
            }

            return View(order);

        }

        public IActionResult CreateEditOrder(Order order)
        {

            if (order.Id > 0)
            {
                _context.Orders.Update(order);
            }
            else
            {

                var orderedItems = order.OrderItems
                    .Where(i => i.Quantity > 0)
                    .ToList();

                order.OrderItems = orderedItems;

                _context.Orders.Add(order);

            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Overview));
        }

        public IActionResult DeleteOrder(int id)
        {

            var order = _context.Orders.FirstOrDefault(o => o.Id == id);

            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Overview));
        }
    }
}
