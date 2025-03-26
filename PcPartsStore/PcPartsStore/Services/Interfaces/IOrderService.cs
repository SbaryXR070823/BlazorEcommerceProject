using Shared.Dto;
using Shared.Models;

namespace PcPartsStore.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersAsync();
        Task<List<Order>> GetUserOrdersAsync(string userId);
        Task<Order> GetOrderByIdAsync(int id);
        Task<OrderDetailsDto> GetOrderDetailsAsync(int id);
        Task<Order> AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task<Order> CreateOrderWithItemsAsync(string userId, List<CartItem> cartItems);
    }
}
