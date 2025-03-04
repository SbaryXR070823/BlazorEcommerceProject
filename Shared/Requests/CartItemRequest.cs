using PcPartsStore;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Requests;

public class CartItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
