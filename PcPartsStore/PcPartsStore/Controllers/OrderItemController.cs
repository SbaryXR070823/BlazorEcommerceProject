using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PcPartsStore.Services.Interfaces;
using Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PcPartsStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;
        private readonly IOrderService _orderService;

        public OrderItemController(IOrderItemService orderItemService, IOrderService orderService)
        {
            _orderItemService = orderItemService;
            _orderService = orderService;
        }

        [HttpGet("order/{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetOrderItemsByOrderId(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
                return NotFound("Order not found");

            var orderItems = await _orderItemService.GetOrderItemsByOrderIdAsync(orderId);
            return Ok(orderItems);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrderItemById(int id)
        {
            var orderItem = await _orderItemService.GetOrderItemByIdAsync(id);
            if (orderItem == null)
                return NotFound();

            return Ok(orderItem);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOrderItem(OrderItem orderItem)
        {
            if (orderItem == null)
                return BadRequest();

            var order = await _orderService.GetOrderByIdAsync(orderItem.OrderId);
            if (order == null)
                return NotFound("Order not found");

            await _orderItemService.AddOrderItemAsync(orderItem);
            return CreatedAtAction(nameof(GetOrderItemById), new { id = orderItem.Id }, orderItem);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderItem(int id, OrderItem orderItem)
        {
            if (id != orderItem.Id)
                return BadRequest();

            var existingOrderItem = await _orderItemService.GetOrderItemByIdAsync(id);
            if (existingOrderItem == null)
                return NotFound();

            await _orderItemService.UpdateOrderItemAsync(orderItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var orderItem = await _orderItemService.GetOrderItemByIdAsync(id);
            if (orderItem == null)
                return NotFound();

            await _orderItemService.DeleteOrderItemAsync(id);
            return NoContent();
        }
    }
}