using Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PcPartsStore.Client.Services
{
    public interface ISpecificationApiService
    {
        Task<List<Specification>> GetSpecificationsAsync();
        Task<Specification> GetSpecificationByIdAsync(int id);
        Task AddSpecificationAsync(Specification specification);
        Task UpdateSpecificationAsync(Specification specification);
        Task DeleteSpecificationAsync(int id);
        Task<List<Specification>> GetSpecificationsByProductIdAsync(int productId);
    }
}