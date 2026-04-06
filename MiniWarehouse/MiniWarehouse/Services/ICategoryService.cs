using MiniWarehouse.Models;

namespace MiniWarehouse.Services
{
    public interface ICategoryService
    {
        List<Category> GetAll();
        Category? GetByName(string name);
        Category? GetById(Guid id);
        ServiceResult<Category> Add(Category category);
        ServiceResult<Category> Update(Guid id, Category updated);
        ServiceResult<bool> Delete(Guid id);
    }
}
