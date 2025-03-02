using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Models;

namespace PcPartsStore.Client.Services
{
    public class CategoryApiService : ICategoryApiService
    {
        private readonly HttpClient _httpClient;

        public CategoryApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Category>>("api/category");
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Category>($"api/category/{id}");
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _httpClient.PostAsJsonAsync("api/category", category);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await _httpClient.PutAsJsonAsync($"api/category/{category.Id}", category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _httpClient.DeleteAsync($"api/category/{id}");
        }
    }
}
