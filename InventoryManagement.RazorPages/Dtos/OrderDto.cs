using System.ComponentModel.DataAnnotations;
using InventoryManagement.RazorPages.Models;

namespace InventoryManagement.RazorPages.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "OrderDate is required.")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [StringLength(500, ErrorMessage = "Comments cannot exceed 500 characters.")]
        public string? Comment { get; set; }

        public IList<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
