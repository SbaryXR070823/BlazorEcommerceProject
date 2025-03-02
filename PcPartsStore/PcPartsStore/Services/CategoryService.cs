using PcPartsStore.Services.Interfaces;
using PcPartsStore.UnitOfWork;
using Shared.Models;

namespace PcPartsStore.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return (await _unitOfWork.Categories.GetAllAsync()).ToList();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _unitOfWork.Categories.GetByIdAsync(id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _unitOfWork.Categories.AddAsync(category);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await _unitOfWork.Categories.UpdateAsync(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _unitOfWork.Categories.DeleteAsync(id);
        }
    }
}
