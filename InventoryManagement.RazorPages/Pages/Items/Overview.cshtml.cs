using InventoryManagement.RazorPages.Data;
using InventoryManagement.RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryManagement.RazorPages.Pages.Items
{
    public class OverviewModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        public IEnumerable<Item> Items { get; set; } = new List<Item>();

        public OverviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Items = _context.Items.ToList();
        }

        public void OnPostDelete(int id)
        {
            var item = _context.Items.Find(id);
            if (item != null)
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
            }

            Items = _context.Items.ToList();
        }
    }
}
