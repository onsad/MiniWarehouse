using MiniWarehouse.Models;

namespace MiniWarehouse.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetProductByIdAsync(Guid guid);
        Task<(ProductServiceResult Result, Product? Product)> AddAsync(ProductCreate p);
        Task<ProductServiceResult> UpdateAsync(Guid id, ProductCreate updated);
        Task<ProductServiceResult> DeleteAsync(Guid id);
        Task<List<Product>> SearchProductsAsync(ProductSearch productSearch);
    }
}
