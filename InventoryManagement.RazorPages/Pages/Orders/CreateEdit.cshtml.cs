using InventoryManagement.RazorPages.Data;
using InventoryManagement.RazorPages.Dtos;
using InventoryManagement.RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.RazorPages.Pages.Orders
{
    public class CreateEditModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        [BindProperty]
        public OrderDto Order { get; set; } = new OrderDto();

        public IEnumerable<Item> AvailableItems { get; set; } = new List<Item>();

        public CreateEditModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet(int? id)
        {

            OrderDto dto = null;


            AvailableItems = _context.Items
                .Where(i => i.Stock > 0)
                .ToList();

            if (id.HasValue)
            {
                var order = _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Item)
                    .First(o => o.Id == id.Value);

                dto = new OrderDto
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    Comment = order.Comment,
                    OrderItems = order.OrderItems.ToList()
                };
            }

            if (dto == null)
            {
                dto = new OrderDto
                {
                    OrderDate = DateTime.Now,
                    Comment = string.Empty
                };

                foreach (var item in AvailableItems)
                {
                    dto.OrderItems.Add(new OrderItem
                    {
                        ItemId = item.Id,
                        //Item = item,
                    });
                }
            }

            Order = dto;

        }

        public IActionResult OnPost()
        {

            int orderId = 0;


            AvailableItems = _context.Items
                .Where(i => i.Stock > 0)
                .ToList();

            if (Order.Id > 0)
            {

                // Update existing order

            }
            else
            {

                // Create new order
                var orderedItems = Order.OrderItems
                    .Where(i => i.Quantity > 0)
                    .Select(i => new OrderItem
                    {
                        ItemId = i.ItemId,
                        Quantity = i.Quantity,
                    })
                    .ToList();

                var order = new Order
                {
                    OrderDate = Order.OrderDate,
                    Comment = Order.Comment,
                    OrderItems = orderedItems
                };

                _context.Orders.Add(order);

                _context.SaveChanges();

                orderId = order.Id;

            }

            // Redirect to the order details page
            return RedirectToPage("/Orders/OrderDetails", new { id = orderId  });

            //RedirectToPage("/Orders/Overview");

        }

    }
}
