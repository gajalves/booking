using BooKing.Reserve.Application.Interfaces;
using BooKing.Reserve.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using BooKing.Generics.Shared.HttpClientService;
using BooKing.Reserve.Domain;
using BooKing.Generics.EventSourcing;
using BooKing.Generics.EventSourcing.Repository;
using BooKing.Generics.EventSourcing.Interfaces;
using BooKing.Generics.EventSourcing.Services;

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
        services.AddTransient<IEventSourcingRepository, EventSourcingRepository>();
        services.AddTransient<IEventStoreService, EventStoreService>();
    }
}
