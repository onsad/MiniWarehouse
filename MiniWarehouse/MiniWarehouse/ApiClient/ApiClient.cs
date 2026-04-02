using System.Net.Http.Json;
using System.Text.Json;

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
                    var content = await response.Content.ReadAsStringAsync();

                    ErrorResponse? error = null;

                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        try
                        {
                            error = JsonSerializer.Deserialize<ErrorResponse>(content);
                        }
                        catch
                        {
                            return new ApiResult<T>
                            {
                                Success = false,
                                Error = error?.Message ?? "Unknown error"
                            };
                        }
                    }

                    return new ApiResult<T>
                    {
                        Success = false,
                        Error = error?.Message ?? "Unknown error"
                    };
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NoContent || response.Content.Headers.ContentLength == 0)
                {
                    return new ApiResult<T>
                    {
                        Success = true,
                        Data = default
                    };
                }

                T? data = default;

                try
                {
                    data = await response.Content.ReadFromJsonAsync<T>();
                }
                catch
                {
                    return new ApiResult<T>
                    {
                        Success = false,
                        Error = "Invalid response from server"
                    };
                }

                return new ApiResult<T>
                {
                    Success = true,
                    Data = data!
                };
            }
            catch (TaskCanceledException)
            {
                return new ApiResult<T>
                {
                    Success = false,
                    Error = "Request timed out"
                };
            }
            catch (HttpRequestException)
            {
                return new ApiResult<T>
                {
                    Success = false,
                    Error = "Cannot reach server"
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
