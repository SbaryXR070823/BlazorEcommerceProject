using PcPartsStore.Services.Interfaces;
using PcPartsStore.UnitOfWork;
using Shared.Models;

namespace PcPartsStore.Services;

public class CartItemsService : ICartItemsService
{
    private readonly IUnitOfWork _unitOfWork;

    public CartItemsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<CartItem>> GetCartItemsAsync()
    {
        return (await _unitOfWork.CartItems.GetAllAsync()).ToList();
    }

    public async Task<CartItem> GetCartItemsByIdAsync(int id)
    {
        return await _unitOfWork.CartItems.GetByIdAsync(id);
    }

    public async Task AddCartItemAsync(CartItem cartItem)
    {
        await _unitOfWork.CartItems.AddAsync(cartItem);
    }

    public async Task UpdateCartItemAsync(CartItem cartItem)
    {
        await _unitOfWork.CartItems.UpdateAsync(cartItem);
    }

    public async Task DeleteCartItemAsync(int id)
    {
        await _unitOfWork.CartItems.DeleteAsync(id);
    }

    public async Task<List<CartItem>> GetCartItemsByUserIdAsync(string userId)
    {
        return (await _unitOfWork.CartItems.FindAsync(c => c.UserId == userId)).ToList();
    }

    public async Task<CartItem?> GetCartItemByProductIdForUser(int productId, string userId)
    {
        return (await _unitOfWork.CartItems.FindAsync(x => x.UserId == userId && x.ProductId == productId)).FirstOrDefault();
    }

    public async Task<int> GetCountOfCartITems(string userId)
    {
        return (await _unitOfWork.CartItems.FindAsync(c => c.UserId == userId)).Count();
    }

}
