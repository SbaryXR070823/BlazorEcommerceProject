using Shared.Models;
using Shared.Requests;

namespace PcPartsStore.Client.Services
{
    public interface ICartItemApiService
    {
        Task<List<CartItem>> GetCartItemsAsync();
        Task<CartItem> GetCartItemByIdAsync(int id);
        Task<List<CartItem>> GetCartItemsByUserIdAsync(string userId);
        Task AddCartItemAsync(CartItemRequest cartItem);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task DeleteCartItemAsync(int id);
        Task<int> GetCountOfCartItemsAsync();

    }
}
