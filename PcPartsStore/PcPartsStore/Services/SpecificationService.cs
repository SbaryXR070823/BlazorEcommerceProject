using PcPartsStore.Services.Interfaces;
using PcPartsStore.UnitOfWork;
using Shared.Models;
using Shared.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PcPartsStore.Services
{
    public class SpecificationService : ISpecificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly LuceneService _luceneService;

        public SpecificationService(IUnitOfWork unitOfWork, LuceneService luceneService)
        {
            _unitOfWork = unitOfWork;
            _luceneService = luceneService;
        }

        public async Task<List<Specification>> GetSpecificationsAsync()
        {
            return (await _unitOfWork.Specifications.GetAllAsync()).ToList();
        }

        public async Task<Specification> GetSpecificationByIdAsync(int id)
        {
            return await _unitOfWork.Specifications.GetByIdAsync(id);
        }

        public async Task AddSpecificationAsync(Specification specification)
        {
            await _unitOfWork.Specifications.AddAsync(specification);
            await UpdateLuceneIndexAsync();
        }

        public async Task UpdateSpecificationAsync(Specification specification)
        {
            await _unitOfWork.Specifications.UpdateAsync(specification);
            await UpdateLuceneIndexAsync();
        }

        public async Task DeleteSpecificationAsync(int id)
        {
            await _unitOfWork.Specifications.DeleteAsync(id);
            await UpdateLuceneIndexAsync();
        }
        public async Task<List<Specification>> GetSpecificationsByProductIdAsync(int productId)
        {
            return (await _unitOfWork.Specifications.FindAsync(s => s.ProductId == productId)).ToList();
        }

        public async Task<List<Specification>> GetSpecificationsByProductIdsAsync(IEnumerable<int> productIds)
        {
            return (await _unitOfWork.Specifications.FindAsync(s => productIds.Contains(s.ProductId))).ToList();
        }

        public async Task<Dictionary<string, List<string>>> GetGroupedSpecificationsForProductsAsync(IEnumerable<int> productIds)
        {
            var specifications = await GetSpecificationsByProductIdsAsync(productIds);

            return specifications
                .GroupBy(s => s.Key)
                .ToDictionary(g => g.Key, g => g.Select(s => s.Value).ToList());
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