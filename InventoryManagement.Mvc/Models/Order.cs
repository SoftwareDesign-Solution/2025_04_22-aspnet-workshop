﻿using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Mvc.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "OrderDate is required.")]
        public DateTime OrderDate { get; set; }

        [StringLength(500, ErrorMessage = "Comments cannot exceed 500 characters.")]
        public string? Comment { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        
    }
}
