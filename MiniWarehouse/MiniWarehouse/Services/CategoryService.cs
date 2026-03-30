using MiniWarehouse.Models;

namespace MiniWarehouse.Services
{
    public class CategoryService
    {
        private readonly List<Category> categories = new()
        {
            new Category { Name = "Ovoce" },
            new Category { Name = "Napoje" },
            new Category { Name = "Pekarna" }
        };

        public Task<List<Category>> GetAllAsync()
        {
            var categories = this.categories.ToList();
            return Task.FromResult(categories);
        }   

        public Task<Category> GetCategoryByName(string name)
        {
            var category = categories.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (category != null)
            {
                return Task.FromResult(category);   
            }
            else
            {
                return Task.FromResult<Category>(null);
            }
        }

        public Task<Category> GetCategoryById(Guid guid)
        {
            var category = categories.FirstOrDefault(c => c.Id == guid);

            if (category != null)
            {
                return Task.FromResult(category);
            }
            else
            {
                return Task.FromResult<Category>(null);
            }
        }

        public Task AddCategoryAsync(Category category)
        {
            categories.Add(category);

            return Task.CompletedTask;
        }

        // Check whether a category with the given id exists
        public bool Exists(Guid id)
        {
            return categories.Any(c => c.Id == id);
        }

        public Task<bool> UpdateCategoryAsync(Guid id, Category category)
        {
            var existing = categories.FirstOrDefault(c => c.Id == id);
            if (existing == null)
                return Task.FromResult(false);

            // Update properties
            existing.Name = category.Name;

            return Task.FromResult(true);
        }

        public Task<bool> DeleteCategoryAsync(Guid id)
        {
            var existing = categories.FirstOrDefault(c => c.Id == id);
            if (existing == null)
                return Task.FromResult(false);

            categories.Remove(existing);

            return Task.FromResult(true);
        }
    }
}
