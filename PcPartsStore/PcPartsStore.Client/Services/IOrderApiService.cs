using Shared.Dto;
using Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PcPartsStore.Client.Services
{
    public interface IOrderApiService
    {
        Task<List<Order>> GetOrdersAsync();
        Task<List<Order>> GetUserOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task<OrderDetailsDto> GetOrderDetailsAsync(int id);
        Task<OrderDetailsDto> CreateOrderAsync();
        Task DeleteOrderAsync(int id);
    }
}