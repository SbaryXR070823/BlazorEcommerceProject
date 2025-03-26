using Microsoft.EntityFrameworkCore;
using PcPartsStore.Services.Interfaces;
using PcPartsStore.UnitOfWork;
using Shared.Models;

namespace PcPartsStore.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            var orderItems = await _unitOfWork.OrderItems.FindAsync(oi => oi.OrderId == orderId);
            return orderItems.ToList();
        }

        public async Task<OrderItem> GetOrderItemByIdAsync(int id)
        {
            return await _unitOfWork.OrderItems.GetByIdAsync(id);
        }

        public async Task AddOrderItemAsync(OrderItem orderItem)
        {
            await _unitOfWork.OrderItems.AddAsync(orderItem);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddOrderItemsAsync(IEnumerable<OrderItem> orderItems)
        {
            foreach (var item in orderItems)
            {
                await _unitOfWork.OrderItems.AddAsync(item);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateOrderItemAsync(OrderItem orderItem)
        {
            await _unitOfWork.OrderItems.UpdateAsync(orderItem);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteOrderItemAsync(int id)
        {
            var orderItem = await _unitOfWork.OrderItems.GetByIdAsync(id);
            if (orderItem != null)
            {
                await _unitOfWork.OrderItems.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}