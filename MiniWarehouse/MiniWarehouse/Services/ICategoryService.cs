using MiniWarehouse.Models;

namespace MiniWarehouse.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetCategoryByName(string name);
        Task<Category?> GetCategoryById(Guid guid);
        Task AddCategoryAsync(Category category);
        Task<bool> ExistsAsync(Guid id);
        Task<CategoryServiceResult> UpdateCategoryAsync(Guid id, Category category);
        Task<CategoryServiceResult> DeleteCategoryAsync(Guid id);
    }
}
