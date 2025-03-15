using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Models;

namespace PcPartsStore.Client.Services;

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

    public async Task<List<Specification>> GetSpecificationsByProductIdAsync(int productId)
    {
        var response = await _httpClient.GetAsync($"api/specification/get/{productId}");
        if (response.IsSuccessStatusCode)
        {
            if (response.Content.Headers.ContentLength == 0)
                return new List<Specification>();

            return await response.Content.ReadFromJsonAsync<List<Specification>>();
        }
        return new List<Specification>();
    }

    public async Task<List<Specification>> GetSpecificationsByProductIdsAsync(IEnumerable<int> productIds)
    {
        var response = await _httpClient.PostAsJsonAsync("api/specification/getbyproductids", productIds);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<Specification>>();
        }
        return new List<Specification>();
    }

    public async Task<Dictionary<string, List<string>>> GetGroupedSpecificationsForProductsAsync(IEnumerable<int> productIds)
    {
        var response = await _httpClient.PostAsJsonAsync("api/specification/getgroupedbyproductids", productIds);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>();
        }
        return new Dictionary<string, List<string>>();
    }
}