using Shared.Models;

namespace PcPartsStore.Services.Interfaces
{
    public interface IOrderItemService
    {
        Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);
        Task<OrderItem> GetOrderItemByIdAsync(int id);
        Task AddOrderItemAsync(OrderItem orderItem);
        Task AddOrderItemsAsync(IEnumerable<OrderItem> orderItems);
        Task UpdateOrderItemAsync(OrderItem orderItem);
        Task DeleteOrderItemAsync(int id);
    }
}