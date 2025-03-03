using PcPartsStore.Client.Services;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PcPartsStore.Client.Services;

public class CartItemApiService : ICartItemApiService
{
    private readonly HttpClient _httpClient;

    public CartItemApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CartItem>> GetCartItemsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<CartItem>>("api/cartitems");
    }

    public async Task<CartItem> GetCartItemByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<CartItem>($"api/cartitems/{id}");
    }

    public async Task<List<CartItem>> GetCartItemsByUserIdAsync(string userId)
    {
        return await _httpClient.GetFromJsonAsync<List<CartItem>>($"api/cartitems/user/{userId}");
    }

    public async Task<int> GetCountOfCartItemsAsync()
    {
        return await _httpClient.GetFromJsonAsync<int>($"api/CartItems/count");
    }

    public async Task AddCartItemAsync(CartItem cartItem)
    {
        await _httpClient.PostAsJsonAsync("api/cartitems", cartItem);
    }

    public async Task UpdateCartItemAsync(CartItem cartItem)
    {
        await _httpClient.PutAsJsonAsync($"api/cartitems/{cartItem.Id}", cartItem);
    }

    public async Task DeleteCartItemAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/cartitems/{id}");
    }
}
