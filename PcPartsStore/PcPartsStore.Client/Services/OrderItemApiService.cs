using Shared.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PcPartsStore.Client.Services
{
    public class OrderItemApiService : IOrderItemApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public OrderItemApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            var response = await _httpClient.GetAsync($"api/orderitem/order/{orderId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<OrderItem>>(_jsonOptions);
        }

        public async Task<OrderItem> GetOrderItemByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/orderitem/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OrderItem>(_jsonOptions);
        }

        public async Task<OrderItem> AddOrderItemAsync(OrderItem orderItem)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(orderItem),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("api/orderitem", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OrderItem>(_jsonOptions);
        }

        public async Task UpdateOrderItemAsync(int id, OrderItem orderItem)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(orderItem),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PutAsync($"api/orderitem/{id}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteOrderItemAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/orderitem/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}