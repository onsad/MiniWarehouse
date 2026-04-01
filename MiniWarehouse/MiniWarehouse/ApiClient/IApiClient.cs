namespace MiniWarehouse.ApiClient
{
    public interface IApiClient
    {
        Task<ApiResult<List<T>>> GetAsync<T>(string url);
        Task<ApiResult<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest payload);
        Task<ApiResult<TResponse>> PutAsync<TRequest, TResponse>(string url, TRequest payload);
        Task<ApiResult<bool>> DeleteAsync(string url);
    }
}
