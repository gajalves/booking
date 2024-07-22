using BooKing.Reserve.Application.Interfaces;
using BooKing.Reserve.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using BooKing.Generics.Shared.HttpClientService;
using BooKing.Reserve.Domain;

namespace BooKing.Reserve.Application;
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
        services.AddHttpClient();
        services.AddHttpClientService();
        services.AddTransient<IReservationService, ReservationService>();
        services.AddTransient<IApartmentService, ApartmentService>();
        services.AddTransient<PricingService>();
    }
}
