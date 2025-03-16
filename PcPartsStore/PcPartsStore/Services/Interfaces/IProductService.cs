using Shared.Models;

namespace PcPartsStore.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync(ProductFilters filters);
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}