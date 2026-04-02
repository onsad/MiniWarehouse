using MiniWarehouse.Models;

namespace MiniWarehouse.Repository
{
    public class DataRepository
    {
        private readonly List<Product> products;
        public List<Product> Products => products;
        public List<Category> Categories => categories;

        public DataRepository()
        {
            products = new List<Product>
            {
                new Product { Id = new Guid("11111111-1111-1111-1111-111111111111"), Name = "Jablko", Category = categories.First(c => c.Name == "Ovoce"), Description = "Cerstve cervene jablko", Price = 10.5m },
                new Product { Id = new Guid("22222222-2222-2222-2222-222222222222"), Name = "Banán", Category = categories.First(c => c.Name == "Ovoce"), Description = "Zraly banan", Price = 8.9m },
                new Product { Id = new Guid("33333333-3333-3333-3333-333333333333"), Name = "Mléko", Category = categories.First(c => c.Name == "Napoje"), Description = "1L polotucne", Price = 25m },
                new Product { Id = new Guid("44444444-4444-4444-4444-444444444444"), Name = "Chleba", Category = categories.First(c => c.Name == "Pekarna"), Description = "Cerny chleba", Price = 30m }
            };
        }

        private readonly List<Category> categories = new()
        {
            new Category { Id = new Guid("e3d9a8e2-1f9a-4b6a-9c3b-1b2d6a7c8e01"), Name = "Ovoce" },
            new Category { Id = new Guid("d1f2a3b4-c5d6-4e7f-8123-4567890abcde"), Name = "Napoje" },
            new Category { Id = new Guid("a0b1c2d3-e4f5-4711-9012-3456789abcde"), Name = "Pekarna" }
        };
    }
}
