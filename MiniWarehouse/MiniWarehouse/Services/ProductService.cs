using MiniWarehouse.Models;
using MiniWarehouse.Repository;

namespace MiniWarehouse.Services
{
    public class ProductService(DataRepository dataRepository)
    {
        public Task<List<Product>> GetAllAsync()
        {
            return Task.FromResult(dataRepository.Products.ToList());
        }

        public Task<Product?> GetProductByIdAsync(Guid guid)
        {
            return Task.FromResult(dataRepository.Products.FirstOrDefault(p => p.Id == guid));
        }

        public async Task<(ProductServiceResult Result, Product? Product)> AddAsync(ProductCreate p)
        {
            var category = dataRepository.Categories.FirstOrDefault(c => c.Id == p.CategoryId);
            if (category == null)
                return (ProductServiceResult.CategoryNotFound, null);

            var prod = new Product
            {
                Id = p.Id,
                Name = p.Name,
                Category = category,
                Description = p.Description,
                Price = p.Price
            };

            dataRepository.Products.Add(prod);

            return (ProductServiceResult.Success, prod);
        }

        public Task<ProductServiceResult> UpdateAsync(Guid id, ProductCreate updated)
        {
            var productToEdit = dataRepository.Products.FirstOrDefault(p => p.Id == id);
            if (productToEdit == null)
                return Task.FromResult(ProductServiceResult.NotFound);

            var category = dataRepository.Categories.FirstOrDefault(c => c.Id == updated.CategoryId);
            if (category == null)
                return Task.FromResult(ProductServiceResult.CategoryNotFound);

            productToEdit.Name = updated.Name;
            productToEdit.Category = category;
            productToEdit.Description = updated.Description;
            productToEdit.Price = updated.Price;

            return Task.FromResult(ProductServiceResult.Success);
        }

        public Task<ProductServiceResult> DeleteAsync(Guid id)
        {
            var existing = dataRepository.Products.FirstOrDefault(p => p.Id == id);
            if (existing == null)
                return Task.FromResult(ProductServiceResult.NotFound);

            dataRepository.Products.Remove(existing);
            return Task.FromResult(ProductServiceResult.Success);
        }

        public Task<List<Product>> SearchProductsAsync(ProductSearch productSearch)
        {
            var query = dataRepository.Products.AsQueryable();

            if (!string.IsNullOrEmpty(productSearch.ProductName))
            {
                query = query.Where(p => p.Name.Contains(productSearch.ProductName, StringComparison.InvariantCultureIgnoreCase));
            }

            if (!string.IsNullOrEmpty(productSearch.Description))
            {
                query = query.Where(p => p.Description.Contains(productSearch.Description, StringComparison.InvariantCultureIgnoreCase));
            }

            if (!string.IsNullOrEmpty(productSearch.CategoryName))
            {
                query = query.Where(p => p.Category.Name.Contains(productSearch.CategoryName, StringComparison.InvariantCultureIgnoreCase));
            }

            return Task.FromResult(query.ToList());
        }
    }
}
