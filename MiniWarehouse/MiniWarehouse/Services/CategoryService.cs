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
            dataRepository.Categories.Add(category);

            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return Task.FromResult(dataRepository.Categories.Any(c => c.Id == id));
        }

        public Task<bool> UpdateCategoryAsync(Guid id, Category category)
        {
            var existing = dataRepository.Categories.FirstOrDefault(c => c.Id == id);
            if (existing == null)
                throw new KeyNotFoundException("Category not found.");

            // Update properties
            existing.Name = category.Name;

            return Task.FromResult(true);
        }

        public Task<bool> DeleteCategoryAsync(Guid id)
        {
            var existing = dataRepository.Categories.FirstOrDefault(c => c.Id == id);
            if (existing == null)
                throw new KeyNotFoundException("Category not found.");

            var productsWithCategory = dataRepository.Products.Any(p => p.Category.Id == id);
            if (productsWithCategory)
                throw new InvalidOperationException("Category is used by products.");

            dataRepository.Categories.Remove(existing);

            return Task.FromResult(true);
        }
    }
}
