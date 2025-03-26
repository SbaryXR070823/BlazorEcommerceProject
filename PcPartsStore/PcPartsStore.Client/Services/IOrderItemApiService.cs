using Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PcPartsStore.Client.Services
{
    public interface IOrderItemApiService
    {
        Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);
        Task<OrderItem> GetOrderItemByIdAsync(int id);
        Task<OrderItem> AddOrderItemAsync(OrderItem orderItem);
        Task UpdateOrderItemAsync(int id, OrderItem orderItem);
        Task DeleteOrderItemAsync(int id);
    }
}