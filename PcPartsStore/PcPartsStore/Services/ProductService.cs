using PcPartsStore.Services.Interfaces;
using PcPartsStore.UnitOfWork;
using Shared.Helpers;
using Shared.Models;

namespace PcPartsStore.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Product>> GetProductsAsync(ProductFilters filters)
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            var filteredProducts = products.AsQueryable();

            if (!string.IsNullOrEmpty(filters.Name))
            {
                var searchTerm = filters.Name.ToLower();
                filteredProducts = filteredProducts.Where(p => StringSearch.KMPContains(p.Name.ToLower(), searchTerm));
            }

            if (!string.IsNullOrEmpty(filters.Category))
            {
                if (int.TryParse(filters.Category, out int categoryId))
                {
                    filteredProducts = filteredProducts.Where(p => p.CategoryId == categoryId);
                }
            }

            if (filters.MinPrice.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => p.Price >= filters.MinPrice.Value);
            }

            if (filters.MaxPrice.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => p.Price <= filters.MaxPrice.Value);
            }

            return filteredProducts.ToList();
        }


        public async Task<Product> GetProductByIdAsync(int id)
        {
            var response = await _unitOfWork.Products.GetByIdAsync(id);
            return response;
        }

        public async Task AddProductAsync(Product product)
        {
            await _unitOfWork.Products.AddAsync(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _unitOfWork.Products.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _unitOfWork.Products.DeleteAsync(id);
        }
    }
}