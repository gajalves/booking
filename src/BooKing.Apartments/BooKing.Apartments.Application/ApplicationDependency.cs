using BooKing.Apartments.Application.Interfaces;
using BooKing.Apartments.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BooKing.Apartments.Application;
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
        services.AddTransient<IApartmentService, ApartmentService>();
        services.AddTransient<IAmenityService, AmenityService>();
    }
}
