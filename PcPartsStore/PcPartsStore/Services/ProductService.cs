using PcPartsStore.Services.Interfaces;
using PcPartsStore.UnitOfWork;
using Shared.Helpers;
using Shared.Models;
using Shared.Services;

namespace PcPartsStore.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly LuceneService _luceneService;

        public ProductService(IUnitOfWork unitOfWork, LuceneService luceneService)
        {
            _unitOfWork = unitOfWork;
            _luceneService = luceneService;
        }

        public async Task<List<Product>> GetProductsAsync(ProductFilters filters)
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            var specifications = await _unitOfWork.Specifications.GetAllAsync();

            // Attach specifications to products
            var specLookup = specifications.GroupBy(s => s.ProductId)
                                            .ToDictionary(g => g.Key, g => g.ToList());
            foreach (var product in products)
            {
                if (specLookup.TryGetValue(product.Id, out var productSpecs))
                {
                    product.Specifications = productSpecs;
                }
                else
                {
                    product.Specifications = new List<Specification>();
                }
            }

            // Filter by name using StringSearch
            if (!string.IsNullOrEmpty(filters.Name))
            {
                products = products.Where(p => StringSearch.KMPContains(p.Name, filters.Name)).ToList();
            }

            // Filter by specifications
            if (filters.Specifications != null && filters.Specifications.Any())
            {
                foreach (var specFilter in filters.Specifications)
                {
                    products = products.Where(p => p.Specifications.Any(s => s.Key == specFilter.Key && s.Value == specFilter.Value)).ToList();
                }
            }

            // Apply other filters (Category, MinPrice, MaxPrice)
            if (!string.IsNullOrEmpty(filters.Category))
            {
                if (int.TryParse(filters.Category, out int categoryId))
                {
                    products = products.Where(p => p.CategoryId == categoryId).ToList();
                }
            }

            if (filters.MinPrice.HasValue)
            {
                products = products.Where(p => p.Price >= filters.MinPrice.Value).ToList();
            }

            if (filters.MaxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= filters.MaxPrice.Value).ToList();
            }

            return products.ToList();
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