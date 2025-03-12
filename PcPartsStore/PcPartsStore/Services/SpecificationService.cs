using PcPartsStore.Services.Interfaces;
using PcPartsStore.UnitOfWork;
using Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PcPartsStore.Services
{
    public class SpecificationService : ISpecificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SpecificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        }

        public async Task UpdateSpecificationAsync(Specification specification)
        {
            await _unitOfWork.Specifications.UpdateAsync(specification);
        }

        public async Task DeleteSpecificationAsync(int id)
        {
            await _unitOfWork.Specifications.DeleteAsync(id);
        }
    }
}