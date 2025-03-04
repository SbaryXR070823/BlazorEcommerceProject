using PcPartsStore.Client.Services;
using Shared.Dto;
using Shared.Models;
using Shared.Requests;
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

    public async Task<CartItem?> GetCartItemByProductIdForUser(int productId)
    {
        var response = await _httpClient.GetAsync($"api/cartitems/get/{productId}");

        if (response.IsSuccessStatusCode)
        {
            if (response.Content.Headers.ContentLength == 0)
                return null;

            return await response.Content.ReadFromJsonAsync<CartItem?>();
        }

        return null;
    }

    public async Task<List<CartProductsDto>> GetCartProductsByUserIdAsync()
    {
        List<CartProductsDto> cartProducts = new List<CartProductsDto>();
        List <CartItem> cartItems = await _httpClient.GetFromJsonAsync<List<CartItem>>($"api/cartitems/user");
        foreach (var cart in cartItems)
        {
            var product = await _httpClient.GetFromJsonAsync<Product>($"api/product/{cart.ProductId}");
            cartProducts.Add(new CartProductsDto
            {
                ProductId = cart.ProductId,
                Quantity = cart.Quantity,
                CartItemId = cart.Id,
                Total = cart.Quantity * product.Price,
                Product = product
            });
        }
        return cartProducts;
    }

    public async Task<List<CartItem>> GetCartItemsByUserIdAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<CartItem>>($"api/cartitems/user");
    }

    public async Task<int> GetCountOfCartItemsAsync()
    {
        return await _httpClient.GetFromJsonAsync<int>($"api/CartItems/count");
    }

    public async Task AddCartItemAsync(CartItemRequest cartItem)
    {
        await _httpClient.PostAsJsonAsync("api/cartitems", cartItem);
    }

    public async Task UpdateCartItemAsync(CartItemRequest cartItem, int id)
    {
        await _httpClient.PutAsJsonAsync($"api/cartitems/{id}", cartItem);
    }

    public async Task DeleteCartItemAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/cartitems/{id}");
    }
}
