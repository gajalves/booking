using Microsoft.Extensions.DependencyInjection;

namespace BooKing.Generics.Shared.HttpClientService;
public static class HttpClientServiceConfiguration
{
    public static void AddHttpClientService(this IServiceCollection services)
    {
        services.AddScoped<IHttpClientService, HttpClientService>();
    }
}
