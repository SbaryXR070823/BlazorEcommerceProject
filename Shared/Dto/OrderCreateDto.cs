using Shared.Models;
using System;
using System.Collections.Generic;

namespace Shared.Dto
{
    public class OrderCreateDto
    {
        public string UserId { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}