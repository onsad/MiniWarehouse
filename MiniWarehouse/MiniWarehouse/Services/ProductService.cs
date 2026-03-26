


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


        public async Task AddAsync(Product p)
        {
            // Intentional small async gap — can lead to duplicates on very quick clicks
            await Task.Delay(120);
            _products.Add(p);
        }


        public Task RemoveByIndexAsync(int index)
        {
            // Intentional bug: removes index+1 instead of index if possible
            var idxToRemove = index + 1;
            if (idxToRemove >= 0 && idxToRemove < _products.Count)
                _products.RemoveAt(idxToRemove);
            return Task.CompletedTask;
        }


        public Task UpdateAsync(Guid id, Product updated)
        {
            // Intentional bug: instead of updating, we add as a new product
            _products.Add(updated);
            return Task.CompletedTask;
        }
    }
}
