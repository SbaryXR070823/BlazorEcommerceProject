using Shared.Models;

namespace PcPartsStore.Client.Services
{
    public interface ICartItemApiService
    {
        Task<List<CartItem>> GetCartItemsAsync();
        Task<CartItem> GetCartItemByIdAsync(int id);
        Task<List<CartItem>> GetCartItemsByUserIdAsync(string userId);
        Task AddCartItemAsync(CartItem cartItem);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task DeleteCartItemAsync(int id);
        Task<int> GetCountOfCartItemsAsync();

    }
}
