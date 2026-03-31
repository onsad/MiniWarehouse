using MiniWarehouse.Models;
using System.Net.Http.Json;

namespace MiniWarehouse.Helpers
{
    public static class ApiHelpers
    {
        public static async Task<ApiResult<List<T>>> SafeGetAsync<T>(HttpClient httpClient, string url)
        {
            try
            {
                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResult<List<T>>
                    {
                        Success = false,
                        Error = $"Server error: {response.StatusCode}"
                    };
                }

                var data = await response.Content.ReadFromJsonAsync<List<T>>();

                return new ApiResult<List<T>>
                {
                    Success = true,
                    Data = data ?? new List<T>()
                };
            }
            catch (Exception ex)
            {
                return new ApiResult<List<T>>
                {
                    Success = false,
                    Error = ex.Message
                };
            }
        }

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
