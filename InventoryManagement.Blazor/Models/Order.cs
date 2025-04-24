using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Blazor.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "OrderDate is required.")]
        [BindProperty, DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }

        [StringLength(500, ErrorMessage = "Comments cannot exceed 500 characters.")]
        [BindProperty]
        public string? Comment { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
