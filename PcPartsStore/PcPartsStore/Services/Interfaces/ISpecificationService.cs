﻿using Shared.Models;
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
    }
}