using InventoryManagement.RazorPages.Data;
using InventoryManagement.RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.RazorPages.Pages.Orders
{
    public class OrderDetailsModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        public Order Order { get; set; } = new Order();

        public OrderDetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet(int? id)
        {

            if (id.HasValue)
            {
                Order = _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Item)
                    .First(o => o.Id == id.Value);
            }

        }
    }
}
