using MiniWarehouse.Models;
using MiniWarehouse.Repository;

namespace MiniWarehouse.Services
{
    public class CategoryService(DataRepository dataRepository) : ICategoryService
    {
        public List<Category> GetAll()
        {
            return dataRepository.Categories.ToList();
        }

        public Category? GetById(Guid id)
        {
            return dataRepository.Categories.FirstOrDefault(c => c.Id == id);
        }

        public Category? GetByName(string name)
        {
            return dataRepository.Categories
                .FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public ServiceResult<Category> Add(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            category.Id = Guid.NewGuid();
            dataRepository.Categories.Add(category);

            return ServiceResult<Category>.Ok(category);
        }

        public ServiceResult<Category> Update(Guid id, Category updated)
        {
            if (updated == null)
                throw new ArgumentNullException(nameof(updated));

            var existing = dataRepository.Categories.FirstOrDefault(c => c.Id == id);
            if (existing == null)
                return ServiceResult<Category>.Fail("NotFound");

            existing.Name = updated.Name;

            return ServiceResult<Category>.Ok(existing);
        }

        public ServiceResult<bool> Delete(Guid id)
        {
            var existing = dataRepository.Categories.FirstOrDefault(c => c.Id == id);
            if (existing == null)
                return ServiceResult<bool>.Fail("NotFound");

            var hasProducts = dataRepository.Products.Any(p => p.Category.Id == id);
            if (hasProducts)
                return ServiceResult<bool>.Fail("HasProducts");

            dataRepository.Categories.Remove(existing);

            return ServiceResult<bool>.Ok(true);
        }
    }
}
