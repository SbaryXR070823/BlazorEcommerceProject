using PcPartsStore.Client.Services;
using Shared.Dto;
using Shared.Models;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class ProductApiService : IProductApiService
{
    private readonly HttpClient _httpClient;

    public ProductApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Product>> GetProductsAsync(ProductFilters filters)
    {
        var queryParameters = new Dictionary<string, string>();

        if (!string.IsNullOrEmpty(filters.Name))
        {
            queryParameters["name"] = filters.Name;
        }
        if (!string.IsNullOrEmpty(filters.Category))
        {
            queryParameters["category"] = filters.Category;
        }
        if (filters.MinPrice.HasValue)
        {
            queryParameters["minPrice"] = filters.MinPrice.Value.ToString(CultureInfo.InvariantCulture);
        }
        if (filters.MaxPrice.HasValue)
        {
            queryParameters["maxPrice"] = filters.MaxPrice.Value.ToString(CultureInfo.InvariantCulture);
        }
        if (filters.Specifications != null && filters.Specifications.Any())
        {
            foreach (var spec in filters.Specifications)
            {
                if (!string.IsNullOrEmpty(spec.Value)) 
                {
                    queryParameters[$"specifications[{spec.Key}]"] = spec.Value;
                }
            }
        }
           
        var queryString = await new FormUrlEncodedContent(queryParameters).ReadAsStringAsync();
        var fullUrl = $"{_httpClient.BaseAddress}api/product?{queryString}";
        Console.WriteLine($"Requesting: {fullUrl}");

        return await _httpClient.GetFromJsonAsync<List<Product>>($"api/product?{queryString}");
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Product>($"api/product/{id}");
    }

    public async Task AddProductAsync(ProductCreateUpdateDto productDto)
    {
        await _httpClient.PostAsJsonAsync("api/product", productDto);
    }

    public async Task UpdateProductAsync(ProductCreateUpdateDto productDto)
    {
        await _httpClient.PutAsJsonAsync($"api/product/{productDto.Id}", productDto);
    }

    public async Task DeleteProductAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/product/{id}");
    }
}