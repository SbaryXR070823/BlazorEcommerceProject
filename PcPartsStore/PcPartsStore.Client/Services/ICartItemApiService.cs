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
        Task<CartItem> GetCartItemByProductIdForUser(int productId);
        Task UpdateCartItemAsync(CartItemRequest cartItem, int id);
        Task DeleteCartItemAsync(int id);
        Task<int> GetCountOfCartItemsAsync();

    }
}
