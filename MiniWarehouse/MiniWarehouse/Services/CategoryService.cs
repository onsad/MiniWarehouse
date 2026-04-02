using MiniWarehouse.Models;
using MiniWarehouse.Repository;

namespace MiniWarehouse.Services
{
    public class CategoryService(DataRepository dataRepository)
    {
        public Task<List<Category>> GetAllAsync()
        {
            var categories = dataRepository.Categories.ToList();
            return Task.FromResult(categories);
        }   

        public Task<Category?> GetCategoryByName(string name)
        {
            return Task.FromResult(dataRepository.Categories.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)));
        }

        public Task<Category?> GetCategoryById(Guid guid)
        {
            return Task.FromResult(dataRepository.Categories.FirstOrDefault(c => c.Id == guid));
        }

        public Task AddCategoryAsync(Category category)
        {
            category.Id = Guid.NewGuid();
            dataRepository.Categories.Add(category);

            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return Task.FromResult(dataRepository.Categories.Any(c => c.Id == id));
        }

        public Task<CategoryServiceResult> UpdateCategoryAsync(Guid id, Category category)
        {
            var existing = dataRepository.Categories.FirstOrDefault(c => c.Id == id);
            if (existing == null)
                return Task.FromResult(CategoryServiceResult.NotFound);

            existing.Name = category.Name;

            return Task.FromResult(CategoryServiceResult.Success);
        }

        public Task<CategoryServiceResult> DeleteCategoryAsync(Guid id)
        {
            var existing = dataRepository.Categories.FirstOrDefault(c => c.Id == id);
            if (existing == null)
                return Task.FromResult(CategoryServiceResult.NotFound);

            var productsWithCategory = dataRepository.Products.Any(p => p.Category.Id == id);
            if (productsWithCategory)
                return Task.FromResult(CategoryServiceResult.HasProducts);

            dataRepository.Categories.Remove(existing);

            return Task.FromResult(CategoryServiceResult.Success);
        }
    }
}
