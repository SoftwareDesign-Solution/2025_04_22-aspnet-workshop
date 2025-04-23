using InventoryManagement.RazorPages.Data;
using InventoryManagement.RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryManagement.RazorPages.Pages.Orders
{
    public class OverviewModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        public IEnumerable<Order> Orders { get; set; } = new List<Order>();

        public OverviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Orders = _context.Orders.ToList();
        }

        public void OnPostDelete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }

            Orders = _context.Orders.ToList();
        }
    }
}
