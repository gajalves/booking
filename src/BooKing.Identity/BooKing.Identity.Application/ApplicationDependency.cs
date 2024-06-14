using BooKing.Identity.Application.Interfaces;
using BooKing.Identity.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BooKing.Identity.Application;
public static class ApplicationDependency
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        AddService(services);

        AddAutoMapper(services);

        return services;
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddSingleton(AutoMapperConfiguration.Create().CreateMapper());
    }

    private static void AddService(IServiceCollection services)
    {
        services.AddTransient<IUserRegisterService, UserRegisterService>();
        services.AddTransient<IUserLoginService, UserLoginService>();
        services.AddTransient<IPasswordService, PasswordService>();
        services.AddTransient<ITokenService, TokenService>();
    }
}
