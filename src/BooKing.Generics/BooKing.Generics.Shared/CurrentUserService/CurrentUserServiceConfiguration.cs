using Microsoft.Extensions.DependencyInjection;

namespace BooKing.Generics.Shared.CurrentUserService;
public static class CurrentUserServiceConfiguration
{
    public static void AddCurrentUserService(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
    }
}
