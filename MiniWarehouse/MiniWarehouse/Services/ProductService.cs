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


        // Check whether a product with the given id exists
        public bool Exists(Guid id)
        {
            return _products.Any(p => p.Id == id);
        }

        // Update product by mapping fields from ProductCreate
        public Task<bool> UpdateAsync(Guid id, ProductCreate updated)
        {
            var productToEdit = _products.FirstOrDefault(p => p.Id == id);
            if (productToEdit == null)
                return Task.FromResult(false);

            var category = _categoryService.GetCategoryById(updated.CategoryId).Result;
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
            var existing = _products.FirstOrDefault(p => p.Id == id);
            if (existing == null)
                return Task.FromResult(false);

            _products.Remove(existing);
            return Task.FromResult(true);
        }

        // Overload used by UI code that passes a full Product instance
        public Task UpdateAsync(Guid id, Product updated)
        {
            var productToEdit = _products.FirstOrDefault(p => p.Id == id);
            if (productToEdit != null)
            {
                productToEdit.Name = updated.Name;
                productToEdit.Category = updated.Category;
                productToEdit.Description = updated.Description;
                productToEdit.Price = updated.Price;
            }

            return Task.CompletedTask;
        }

        public Task<List<Product>> SearchProductsAsync(ProductSearch productSearch)
        {
            var query = _products;

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
