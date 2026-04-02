using MiniWarehouse.ApiClient;
using MiniWarehouse.Helpers;
using MiniWarehouse.Models;

namespace MiniWarehouse.ViewModels
{
    public class HomeViewModel
    {
        private readonly IApiClient api;

        public List<Product> Products { get; private set; } = new();
        public List<Category> Categories { get; private set; } = new();

        public string? ErrorMessage { get; private set; }

        public HomeViewModel(IApiClient api)
        {
            this.api = api;
        }

        public async Task<bool> LoadProducts()
        {
            var result = await api.GetAsync<Product>("api/Product");

            if (!result.Success || result.Data == null)
            {
                ErrorMessage = result.Error ?? "Neznámá chyba";
                return false;
            }

            Products = result.Data;
            return true;
        }

        public async Task<bool> LoadCategories()
        {
            var result = await api.GetAsync<Category>("api/Category");

            if (!result.Success || result.Data == null)
            {
                ErrorMessage = result.Error ?? "Neznámá chyba";
                return false;
            }

            Categories = result.Data;
            return true;
        }

        public async Task<bool> CreateProduct(ProductCreate product)
        {
            var result = await api.PostAsync<ProductCreate, Product>("api/Product", product);

            if (!result.Success || result.Data == null)
            {
                ErrorMessage = result.Error ?? "Neznámá chyba";
                return false;
            }

            Products.Add(result.Data);
            return true;
        }

        public async Task<bool> DeleteProduct(Guid id)
        {
            var result = await api.DeleteAsync($"api/Product/{id}");

            if (!result.Success)
            {
                ErrorMessage = result.Error ?? "Neznámá chyba";
                return false;
            }

            Products.RemoveAll(p => p.Id == id);
            return true;
        }

        public async Task<bool> UpdateProduct(ProductCreate product)
        {
            var result = await api.PutAsync<ProductCreate, Product>(
                $"api/Product/{product.Id}", product);

            if (!result.Success)
            {
                ErrorMessage = result.Error ?? "Neznámá chyba";
                return false;
            }

            var existing = Products.FirstOrDefault(p => p.Id == product.Id);
            if (existing != null)
            {
                existing.Name = product.Name;
                existing.Description = product.Description;
                existing.Price = product.Price;
                existing.Category = Categories.First(c => c.Id == product.CategoryId);
            }

            return true;
        }

        public async Task<bool> SearchProducts(string searchTerm, string? categoryName)
        {
            var search = new ProductSearch()
            {
                ProductName = string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm,
                CategoryName = string.IsNullOrWhiteSpace(categoryName) ? null : categoryName
            };

            var query = ApiHelpers.BuildSearchQuery(search);

            var result = await api.GetAsync<Product>($"api/Product/search?{query}");

            if (!result.Success || result.Data == null)
            {
                ErrorMessage = result.Error ?? "Neznámá chyba";
                return false;
            }

            Products = result.Data;
            return true;
        }

        public async Task<bool> CreateCategory(Category category)
        {
            var result = await api.PostAsync<Category, Category>("api/Category", category);

            if (!result.Success || result.Data == null)
            {
                ErrorMessage = result.Error ?? "Neznámá chyba";
                return false;
            }

            Categories.Add(result.Data);
            return true;
        }

        public async Task<bool> DeleteCategory(Guid id)
        {
            var result = await api.DeleteAsync($"api/Category/{id}");

            if (!result.Success)
            {
                ErrorMessage = result.Error ?? "Neznámá chyba";
                return false;
            }

            Categories.RemoveAll(p => p.Id == id);
            return true;
        }

        public string TotalDisplay
        {
            get
            {
                decimal total = Products.Sum(x => x.Price);
                return total.ToString("0.00") + " Kč";
            }
        }
    }
}
