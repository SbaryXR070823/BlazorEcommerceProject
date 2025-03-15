using Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PcPartsStore.Services.Interfaces
{
    public interface ISpecificationService
    {
        Task<List<Specification>> GetSpecificationsAsync();
        Task<Specification> GetSpecificationByIdAsync(int id);
        Task AddSpecificationAsync(Specification specification);
        Task UpdateSpecificationAsync(Specification specification);
        Task DeleteSpecificationAsync(int id);
        Task<List<Specification>> GetSpecificationsByProductIdAsync(int productId);
        Task<Dictionary<string, List<string>>> GetGroupedSpecificationsForProductsAsync(IEnumerable<int> productIds);
        Task<List<Specification>> GetSpecificationsByProductIdsAsync(IEnumerable<int> productIds);
    }
}