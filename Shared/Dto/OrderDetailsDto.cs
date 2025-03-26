using Shared.Models;
using System;
using System.Collections.Generic;

namespace Shared.Dto
{
    public class OrderDetailsDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }

    public class OrderItemDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal => ProductPrice * Quantity;
    }
}