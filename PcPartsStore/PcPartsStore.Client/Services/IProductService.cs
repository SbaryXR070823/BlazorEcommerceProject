using Shared.Dto;
using Shared.Models;

namespace PcPartsStore.Client.Services
{
    public interface IProductApiService
    {
        Task<List<Product>> GetProductsAsync(ProductFilters filters);
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(ProductCreateUpdateDto productDto);
        Task UpdateProductAsync(ProductCreateUpdateDto productDto);
        Task DeleteProductAsync(int id);
    }
}
