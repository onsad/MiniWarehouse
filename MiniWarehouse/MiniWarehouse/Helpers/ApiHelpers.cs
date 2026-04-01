using MiniWarehouse.Models;

namespace MiniWarehouse.Helpers
{
    public static class ApiHelpers
    {
        public static string BuildSearchQuery(ProductSearch search)
        {
            var query = new List<string>();

            if (!string.IsNullOrWhiteSpace(search.ProductName))
                query.Add($"ProductName={Uri.EscapeDataString(search.ProductName)}");

            if (!string.IsNullOrWhiteSpace(search.Description))
                query.Add($"Description={Uri.EscapeDataString(search.Description)}");

            if (!string.IsNullOrWhiteSpace(search.CategoryName))
                query.Add($"CategoryName={Uri.EscapeDataString(search.CategoryName)}");

            return string.Join("&", query);
        }
    }
}
