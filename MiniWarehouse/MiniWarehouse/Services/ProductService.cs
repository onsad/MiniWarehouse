


using MiniWarehouse.Models;

namespace MiniWarehouse.Services
{
    public class ProductService
    {
        // In-memory storage
        private readonly List<Product> _products = new()
{
new Product { Name = "Jablko", Category = "Ovoce", Description = "Cerstve cervene jablko", Price = 10.5m },
new Product { Name = "Banán", Category = "Ovoce", Description = "Zraly banan", Price = 8.9m },
new Product { Name = "Mléko", Category = "Napoje", Description = "1L polotucne", Price = 25m },
new Product { Name = "Chleba", Category = "Pekarna", Description = "Cerny chleba", Price = 30m }
};


        public Task<List<Product>> GetAllAsync()
        {
            // simulated small delay
            return Task.FromResult(_products.ToList());
        }


        public Task AddAsync(Product p)
        {
            _products.Add(p);

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
