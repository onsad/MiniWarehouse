using MiniWarehouse.Models;

namespace MiniWarehouse.Services
{
    public class CategoryService
    {
        private readonly List<Category> categories = new()
        {
            new Category { Id = new Guid("e3d9a8e2-1f9a-4b6a-9c3b-1b2d6a7c8e01"), Name = "Ovoce" },
            new Category { Id = new Guid("d1f2a3b4-c5d6-4e7f-8123-4567890abcde"), Name = "Napoje" },
            new Category { Id = new Guid("a0b1c2d3-e4f5-4711-9012-3456789abcde"), Name = "Pekarna" }
        };

        public Task<List<Category>> GetAllAsync()
        {
            var categories = this.categories.ToList();
            return Task.FromResult(categories);
        }   

        public Task<Category?> GetCategoryByName(string name)
        {
            return Task.FromResult(categories.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)));
        }

        public Task<Category?> GetCategoryById(Guid guid)
        {
            return Task.FromResult(categories.FirstOrDefault(c => c.Id == guid));
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
