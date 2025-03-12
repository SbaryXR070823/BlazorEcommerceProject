using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Models;

namespace PcPartsStore.Client.Services
{
    public class SpecificationApiService : ISpecificationApiService
    {
        private readonly HttpClient _httpClient;

        public SpecificationApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Specification>> GetSpecificationsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Specification>>("api/specification");
        }

        public async Task<Specification> GetSpecificationByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Specification>($"api/specification/{id}");
        }

        public async Task AddSpecificationAsync(Specification specification)
        {
            await _httpClient.PostAsJsonAsync("api/specification", specification);
        }

        public async Task UpdateSpecificationAsync(Specification specification)
        {
            await _httpClient.PutAsJsonAsync($"api/specification/{specification.Id}", specification);
        }

        public async Task DeleteSpecificationAsync(int id)
        {
            await _httpClient.DeleteAsync($"api/specification/{id}");
        }
    }
}