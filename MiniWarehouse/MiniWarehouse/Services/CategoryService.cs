using MiniWarehouse.Models;

namespace MiniWarehouse.Services
{
    public class CategoryService
    {
        private readonly List<Category> _categories = new()
        {
            new Category { Name = "Ovoce" },
            new Category { Name = "Napoje" },
            new Category { Name = "Pekarna" }
        };

        public Task<List<Category>> GetAllAsync()
        {
            var categories = _categories.ToList();
            return Task.FromResult(categories);
        }   

        public Task<Category> GetCategoryByName(string name)
        {
            var category = _categories.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (category != null)
            {
                return Task.FromResult(category);   
            }
            else
            {
                throw new KeyNotFoundException($"Kategorie s názvem '{name}' nebyla nalezena.");
            }
        }   
    }
}
