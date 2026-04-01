using System.Net.Http.Json;

namespace MiniWarehouse.ApiClient
{
    public class ApiClient(HttpClient httpClient) : IApiClient
    {
        
        public async Task<ApiResult<List<T>>> GetAsync<T>(string url)
        {
            return await SendAsync<List<T>>(() => httpClient.GetAsync(url));
        }

        public async Task<ApiResult<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest payload)
        {
            return await SendAsync<TResponse>(() => httpClient.PostAsJsonAsync(url, payload));
        }

        public async Task<ApiResult<TResponse>> PutAsync<TRequest, TResponse>(string url, TRequest payload)
        {
            return await SendAsync<TResponse>(() => httpClient.PutAsJsonAsync(url, payload));
        }

        public async Task<ApiResult<bool>> DeleteAsync(string url)
        {
            var result = await SendAsync<object>(() => httpClient.DeleteAsync(url));

            return new ApiResult<bool>
            {
                Success = result.Success,
                Error = result.Error,
                Data = result.Success
            };
        }

        private async Task<ApiResult<T>> SendAsync<T>(Func<Task<HttpResponseMessage>> httpCall)
        {
            try
            {
                var response = await httpCall();

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

                    return new ApiResult<T>
                    {
                        Success = false,
                        Error = error?.Message ?? "Unknown error"
                    };
                }

                var data = await response.Content.ReadFromJsonAsync<T>();

                return new ApiResult<T>
                {
                    Success = true,
                    Data = data!
                };
            }
            catch (Exception ex)
            {
                return new ApiResult<T>
                {
                    Success = false,
                    Error = ex.Message
                };
            }
        }
    }
}
