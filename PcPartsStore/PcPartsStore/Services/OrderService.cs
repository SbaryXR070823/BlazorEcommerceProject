using Microsoft.EntityFrameworkCore;
using PcPartsStore.Services.Interfaces;
using PcPartsStore.UnitOfWork;
using Shared.Dto;
using Shared.Models;

namespace PcPartsStore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            return orders.ToList();
        }

        public async Task<List<Order>> GetUserOrdersAsync(string userId)
        {
            var orders = await _unitOfWork.Orders.FindAsync(o => o.UserId == userId);
            return orders.ToList();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _unitOfWork.Orders.GetByIdAsync(id);
        }

        public async Task<OrderDetailsDto> GetOrderDetailsAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
                return null;

            var orderItems = await _unitOfWork.OrderItems.FindAsync(oi => oi.OrderId == id);
            
            var orderDetailsDto = new OrderDetailsDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderItems = new List<OrderItemDto>()
            };

            foreach (var item in orderItems)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    orderDetailsDto.OrderItems.Add(new OrderItemDto
                    {
                        Id = item.Id,
                        OrderId = item.OrderId,
                        ProductId = item.ProductId,
                        ProductName = product.Name,
                        ProductImage = product.ImageBase64,
                        ProductPrice = product.Price,
                        Quantity = item.Quantity
                    });
                }
            }

            return orderDetailsDto;
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return order;
        }

        public async Task UpdateOrderAsync(Order order)
        {
            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order != null)
            {
                await _unitOfWork.Orders.UpdateAsync(order);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<Order> CreateOrderWithItemsAsync(string userId, List<CartItem> cartItems)
        {
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = cartItems.Sum(ci => ci.Quantity * ci.Product.Price)
            };
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity
                };

                await _unitOfWork.OrderItems.AddAsync(orderItem);
            }

            await _unitOfWork.SaveChangesAsync();
            return order;
        }
    }
}
