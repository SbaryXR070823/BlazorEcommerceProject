using Shared.Dto;
using Shared.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PcPartsStore.Client.Services
{
    public class OrderApiService : IOrderApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public OrderApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            var response = await _httpClient.GetAsync("api/order");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Order>>(_jsonOptions);
        }

        public async Task<List<Order>> GetUserOrdersAsync()
        {
            var response = await _httpClient.GetAsync("api/order/user");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Order>>(_jsonOptions);
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/order/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Order>(_jsonOptions);
        }
        
        public async Task<OrderDetailsDto> GetOrderDetailsAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/order/details/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OrderDetailsDto>(_jsonOptions);
        }

        public async Task<OrderDetailsDto> CreateOrderAsync()
        {
            var response = await _httpClient.PostAsync("api/order", null);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(response);
            return await response.Content.ReadFromJsonAsync<OrderDetailsDto>();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/order/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}