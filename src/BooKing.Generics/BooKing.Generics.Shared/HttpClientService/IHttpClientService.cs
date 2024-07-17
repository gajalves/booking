namespace BooKing.Generics.Shared.HttpClientService;
public interface IHttpClientService
{
    void SetBearerToken(string token);
    Task<TResponse> GetAsync<TResponse>(string baseUrl, string endpoint);
    Task<TResponse> PostAsync<TRequest, TResponse>(string baseUrl, string endpoint, TRequest data);
    Task<TResponse> PutAsync<TRequest, TResponse>(string baseUrl, string endpoint, TRequest data);
    Task<TResponse> DeleteAsync<TResponse>(string baseUrl, string endpoint);
}
