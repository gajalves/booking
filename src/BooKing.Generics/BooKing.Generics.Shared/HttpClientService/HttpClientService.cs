using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BooKing.Generics.Shared.HttpClientService;
public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;
    private string _bearerToken;

    public HttpClientService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public void SetBearerToken(string token)
    {
        _bearerToken = token;
    }

    private void AddBearerToken()
    {
        if (!string.IsNullOrEmpty(_bearerToken))
        {            
            _httpClient.DefaultRequestHeaders.Add("Authorization", _bearerToken);
        }
    }

    public async Task<TResponse> GetAsync<TResponse>(string baseUrl, string endpoint)
    {

        try
        {
            AddBearerToken();
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(content);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string baseUrl, string endpoint, TRequest data)
    {
        AddBearerToken();
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(endpoint, content);
        response.EnsureSuccessStatusCode();
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TResponse>(responseJson);
    }

    public async Task<TResponse> PutAsync<TRequest, TResponse>(string baseUrl, string endpoint, TRequest data)
    {
        AddBearerToken();
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync(endpoint, content);
        response.EnsureSuccessStatusCode();
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TResponse>(responseJson);
    }

    public async Task<TResponse> DeleteAsync<TResponse>(string baseUrl, string endpoint)
    {
        AddBearerToken();
        var response = await _httpClient.DeleteAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TResponse>(content);
    }
}
