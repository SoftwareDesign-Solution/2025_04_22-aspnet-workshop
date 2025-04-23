using InventoryManagement.RazorPages.Data;
using InventoryManagement.RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryManagement.RazorPages.Pages.Items
{
    public class CreateEditModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        [BindProperty]
        public Item Item { get; set; } = new Item();

        public int SelectedId { get; set; }

        public CreateEditModel(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public void OnGet(int? id)
        {
            SelectedId = id.GetValueOrDefault();

            if (id.HasValue)
            {
                Item = _context.Items.First(i => id == id.Value);
            }
        }

        public IActionResult OnPost(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Item.Id > 0)
            {
                _context.Items.Update(Item);
            }
            else
            {
                _context.Items.Add(Item);
            }

            _context.SaveChanges();

            return RedirectToPage("Overview");
            
        }
    }
}
