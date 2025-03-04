using Shared.Models;

namespace PcPartsStore.Services.Interfaces;

public interface ICartItemsService
{
    Task<List<CartItem>> GetCartItemsAsync();
    Task<CartItem> GetCartItemsByIdAsync(int id);
    Task AddCartItemAsync(CartItem cartItem);
    Task<CartItem?> GetCartItemByProductIdForUser(int productId, string userId);
    Task UpdateCartItemAsync(CartItem cartItem);
    Task DeleteCartItemAsync(int id);
    Task<List<CartItem>> GetCartItemsByUserIdAsync(string userId);
    Task<int> GetCountOfCartITems(string userId);
}
