using MiniWarehouse.Models;
using MiniWarehouse.Repository;

namespace MiniWarehouse.Services
{
    public class ProductService(DataRepository dataRepository) : IProductService
    {
        public List<Product> GetAll()
        {
            return dataRepository.Products.ToList();
        }

        public Product? GetById(Guid guid)
        {
            return dataRepository.Products.FirstOrDefault(p => p.Id == guid);
        }

        public ServiceResult<Product> Add(ProductCreate p)
        {
            var category = dataRepository.Categories.FirstOrDefault(c => c.Id == p.CategoryId);
            if (category == null)
                return ServiceResult<Product>.Fail("Category not found");

            var prod = new Product
            {
                Id = Guid.NewGuid(),
                Name = p.Name,
                Category = category,
                Description = p.Description,
                Price = p.Price
            };

            dataRepository.Products.Add(prod);

            return ServiceResult<Product>.Ok(prod);
        }

        public ServiceResult<Product> Update(Guid id, ProductCreate updated)
        {
            if (updated == null)
                throw new ArgumentNullException(nameof(updated));

            var productToEdit = dataRepository.Products.FirstOrDefault(p => p.Id == id);
            if (productToEdit == null)
                return ServiceResult<Product>.Fail("NotFound");

            var category = dataRepository.Categories.FirstOrDefault(c => c.Id == updated.CategoryId);
            if (category == null)
                return ServiceResult<Product>.Fail("CategoryNotFound");

            productToEdit.Name = updated.Name;
            productToEdit.Category = category;
            productToEdit.Description = updated.Description;
            productToEdit.Price = updated.Price;

            return ServiceResult<Product>.Ok(productToEdit);
        }

        public ServiceResult<bool> Delete(Guid id)
        {
            var existing = dataRepository.Products.FirstOrDefault(p => p.Id == id);
            if (existing == null)
                return ServiceResult<bool>.Fail("NotFound");

            dataRepository.Products.Remove(existing);

            return ServiceResult<bool>.Ok(true);
        }

        public List<Product> SearchProducts(ProductSearch productSearch)
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

            return query.ToList();
        }
    }
}
