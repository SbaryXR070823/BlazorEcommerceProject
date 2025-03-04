using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    public class CartProductsDto
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }   
        public decimal Total { get; set; }
        public int ProductId { get; set; }
        public int CartItemId { get; set; }
    }
}
