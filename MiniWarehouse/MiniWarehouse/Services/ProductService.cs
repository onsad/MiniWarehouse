using MiniWarehouse.Models;

namespace MiniWarehouse.Services
{
    public class ProductService
    {
        private readonly CategoryService categoryService;
        // In-memory storage
        private readonly List<Product> products;

        public ProductService(CategoryService categoryService)
        {
            this.categoryService = categoryService;
            // initialize seed products using the category service
            products = new()
            {
                new Product { Name = "Jablko", Category = this.categoryService.GetCategoryByName("Ovoce").Result, Description = "Cerstve cervene jablko", Price = 10.5m },
                new Product { Name = "Banán", Category = this.categoryService.GetCategoryByName("Ovoce").Result, Description = "Zraly banan", Price = 8.9m },
                new Product { Name = "Mléko", Category = this.categoryService.GetCategoryByName("Napoje").Result, Description = "1L polotucne", Price = 25m },
                new Product { Name = "Chleba", Category = this.categoryService.GetCategoryByName("Pekarna").Result, Description = "Cerny chleba", Price = 30m }
            };
        }

        public Task<List<Product>> GetAllAsync()
        {
            // simulated small delay
            return Task.FromResult(products.ToList());
        }

        public Task<Product?> GetProductByIdAsync(Guid guid)
        {
            return Task.FromResult(products.FirstOrDefault(p => p.Id == guid));
        }

        public Task AddAsync(ProductCreate p)
        {
            // Convert ProductCreate to Product and add
            var category = categoryService.GetCategoryById(p.CategoryId).Result;
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            var prod = new Product
            {
                Id = p.Id,
                Name = p.Name,
                Category = category,
                Description = p.Description,
                Price = p.Price
            };

            products.Add(prod);

            return Task.CompletedTask;
        }

        public bool Exists(Guid id)
        {
            return products.Any(p => p.Id == id);
        }

        public Task<bool> UpdateAsync(Guid id, ProductCreate updated)
        {
            var productToEdit = products.FirstOrDefault(p => p.Id == id);
            if (productToEdit == null)
                return Task.FromResult(false);

            var category = categoryService.GetCategoryById(updated.CategoryId).Result;
            if (category == null)
                return Task.FromResult(false);

            productToEdit.Name = updated.Name;
            productToEdit.Category = category;
            productToEdit.Description = updated.Description;
            productToEdit.Price = updated.Price;

            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            var existing = products.FirstOrDefault(p => p.Id == id);
            if (existing == null)
                return Task.FromResult(false);

            products.Remove(existing);
            return Task.FromResult(true);
        }

        public Task<List<Product>> SearchProductsAsync(ProductSearch productSearch)
        {
            var query = products;

            if (!string.IsNullOrEmpty(productSearch.ProductName))
            {
                query = query.Where(p => p.Name.Contains(productSearch.ProductName)).ToList();
            }

            if (!string.IsNullOrEmpty(productSearch.Description))
            {
                query = query.Where(p => p.Description.Contains(productSearch.Description)).ToList();
            }

            if (!string.IsNullOrEmpty(productSearch.CategoryName))
            {
                query = query.Where(p => p.Category.Name.Contains(productSearch.CategoryName)).ToList();
            }

            return Task.FromResult(query.ToList());
        }
    }
}
