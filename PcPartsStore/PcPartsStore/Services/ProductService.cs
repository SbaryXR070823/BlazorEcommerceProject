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

            _luceneService.CreateIndex(products);

            if (!string.IsNullOrEmpty(filters.Name))
            {
                products = products.Where(p => StringSearch.KMPContains(p.Name, filters.Name)).ToList();
            }

            if (filters.Specifications != null && filters.Specifications.Any())
            {
                var nonEmptySpecs = filters.Specifications
                    .Where(spec => !string.IsNullOrEmpty(spec.Value))
                    .ToDictionary(spec => spec.Key, spec => spec.Value);

                Console.WriteLine("Specification Filters:");
                foreach (var spec in nonEmptySpecs)
                {
                    Console.WriteLine($"{spec.Key}: {spec.Value}");
                }

                if (nonEmptySpecs.Any())
                {
                    var luceneResults = _luceneService.Search(nonEmptySpecs);
                    var luceneProductIds = luceneResults.Select(p => p.Id).ToHashSet();
                    products = products.Where(p => luceneProductIds.Contains(p.Id)).ToList();
                }
            }

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

        public async Task<Product> AddProductAsync(Product product)
        {
            var addedProduct = await _unitOfWork.Products.AddAsync(product);
            await UpdateLuceneIndexAsync();
            return addedProduct;
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _unitOfWork.Products.UpdateAsync(product);
            await UpdateLuceneIndexAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            await _unitOfWork.Products.DeleteAsync(id);
            await UpdateLuceneIndexAsync();
        }

        private async Task UpdateLuceneIndexAsync()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            var specifications = await _unitOfWork.Specifications.GetAllAsync();

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
            _luceneService.CreateIndex(products);
        }
    }
}