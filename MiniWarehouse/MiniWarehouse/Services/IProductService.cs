using MiniWarehouse.Models;

namespace MiniWarehouse.Services
{
    public interface IProductService
    {
        List<Product> GetAll();
        Product? GetById(Guid guid);
        ServiceResult<Product> Add(ProductCreate p);
        ServiceResult<Product> Update(Guid id, ProductCreate updated);
        ServiceResult<bool> Delete(Guid id);
        List<Product> SearchProducts(ProductSearch productSearch);
    }
}
