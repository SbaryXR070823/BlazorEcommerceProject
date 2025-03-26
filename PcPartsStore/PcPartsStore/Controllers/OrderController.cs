using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PcPartsStore.Services.Interfaces;
using Shared.Dto;
using Shared.Models;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PcPartsStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICartItemsService _cartItemsService;
        private readonly IProductService _productService;

        public OrderController(IOrderService orderService, ICartItemsService cartItemsService, IProductService productService)
        {
            _orderService = orderService;
            _cartItemsService = cartItemsService;
            _productService = productService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            if (!User.IsInRole("Admin") && order.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            return Ok(order);
        }

        [HttpGet("details/{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            var orderDetails = await _orderService.GetOrderDetailsAsync(id);
            if (orderDetails == null)
                return NotFound();

            if (!User.IsInRole("Admin") && orderDetails.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            return Ok(orderDetails);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var cartItems = await _cartItemsService.GetCartItemsByUserIdAsync(userId);
            if (cartItems == null || !cartItems.Any())
                return BadRequest("No items in cart");
            foreach(CartItem cartItem in cartItems)
            {
                cartItem.Product = await _productService.GetProductByIdAsync(cartItem.ProductId);
            }
            var order = await _orderService.CreateOrderWithItemsAsync(userId, cartItems);

            foreach (var item in cartItems)
            {
                await _cartItemsService.DeleteCartItemAsync(item.Id);
            }
            var newOrderItems = new List<OrderItemDto>();
            foreach(var item in order.OrderItems)
            {
                item.Product = await _productService.GetProductByIdAsync(item.ProductId);
                newOrderItems.Add(new OrderItemDto
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    ProductId = item.ProductId,
                    ProductImage = item.Product.ImageBase64,
                    ProductName = item.Product.Name,
                    ProductPrice = item.Product.Price,
                    Quantity = item.Quantity
                });
            }
            return Ok(new OrderDetailsDto
            {
                TotalAmount = order.TotalAmount,
                Id = order.Id,
                OrderDate = order.OrderDate,
                UserId = order.UserId,
                OrderItems = newOrderItems
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            await _orderService.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}