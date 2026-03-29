using MiniWarehouse.Models;

namespace MiniWarehouse.Services
{
    public class ProductService
    {
        private readonly CategoryService _categoryService;
        // In-memory storage
        private readonly List<Product> _products;

        public ProductService(CategoryService categoryService)
        {
            _categoryService = categoryService;
            // initialize seed products using the category service
            _products = new()
            {
                new Product { Name = "Jablko", Category = _categoryService.GetCategoryByName("Ovoce").Result, Description = "Cerstve cervene jablko", Price = 10.5m },
                new Product { Name = "Banán", Category = _categoryService.GetCategoryByName("Ovoce").Result, Description = "Zraly banan", Price = 8.9m },
                new Product { Name = "Mléko", Category = _categoryService.GetCategoryByName("Napoje").Result, Description = "1L polotucne", Price = 25m },
                new Product { Name = "Chleba", Category = _categoryService.GetCategoryByName("Pekarna").Result, Description = "Cerny chleba", Price = 30m }
            };
        }


        public Task<List<Product>> GetAllAsync()
        {
            // simulated small delay
            return Task.FromResult(_products.ToList());
        }

        public Task<Product> GetProductByIdAsync(Guid guid)
        {
            var product = _products.FirstOrDefault(p => p.Id == guid);
            if (product != null)
            {
                return Task.FromResult(product);
            }
            
            return Task.FromResult<Product>(null);
        }


        public Task AddAsync(Product p)
        {
            _products.Add(p);

            return Task.CompletedTask;
        }

        public Task AddAsync(ProductCreate p)
        {
            // Convert ProductCreate to Product and add
            var category = _categoryService.GetCategoryById(p.CategoryId).Result;
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

            _products.Add(prod);

            return Task.CompletedTask;
        }


        public Task RemoveByIndexAsync(int index)
        {
            if (index >= 0 && index < _products.Count)
                _products.RemoveAt(index);
            return Task.CompletedTask;
        }


        public Task UpdateAsync(Guid id, Product updated)
        {
            var productToEdit = _products.FirstOrDefault(p => p.Id == id);
            if (productToEdit != null)
            {
                productToEdit = updated;
            }

            return Task.CompletedTask;
        }
    }
}
