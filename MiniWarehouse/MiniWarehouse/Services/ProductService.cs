using MiniWarehouse.Models;
using MiniWarehouse.Repository;

namespace MiniWarehouse.Services
{
    public class ProductService(DataRepository dataRepository)
    {
        public Task<List<Product>> GetAllAsync()
        {
            // simulated small delay
            return Task.FromResult(dataRepository.Products.ToList());
        }

        public Task<Product?> GetProductByIdAsync(Guid guid)
        {
            return Task.FromResult(dataRepository.Products.FirstOrDefault(p => p.Id == guid));
        }

        public Task AddAsync(ProductCreate p)
        {
            // Convert ProductCreate to Product and add
            var category = dataRepository.Categories.FirstOrDefault(c => c.Id == p.CategoryId);
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

            dataRepository.Products.Add(prod);

            return Task.CompletedTask;
        }

        public bool Exists(Guid id)
        {
            return dataRepository.Products.Any(p => p.Id == id);
        }

        public Task<bool> UpdateAsync(Guid id, ProductCreate updated)
        {
            var productToEdit = dataRepository.Products.FirstOrDefault(p => p.Id == id);
            if (productToEdit == null)
                throw new KeyNotFoundException("Product not found.");

            var category = dataRepository.Categories.FirstOrDefault(c => c.Id == updated.CategoryId);
            if (category == null)
                throw new KeyNotFoundException("Category not found.");

            productToEdit.Name = updated.Name;
            productToEdit.Category = category;
            productToEdit.Description = updated.Description;
            productToEdit.Price = updated.Price;

            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            var existing = dataRepository.Products.FirstOrDefault(p => p.Id == id);
            if (existing == null)
                throw new KeyNotFoundException("Product not found.");

            dataRepository.Products.Remove(existing);
            return Task.FromResult(true);
        }

        public Task<List<Product>> SearchProductsAsync(ProductSearch productSearch)
        {
            var query = dataRepository.Products;

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
