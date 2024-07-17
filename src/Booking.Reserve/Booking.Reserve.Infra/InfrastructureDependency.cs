using BooKing.Generics.Infra.Implementations;
using BooKing.Generics.Infra;
using Microsoft.Extensions.DependencyInjection;
using Booking.Reserve.Infra.Context;
using Booking.Reserve.Domain.Interfaces;
using Booking.Reserve.Infra.Repositories;

namespace Booking.Reserve.Infra;
public static class InfrastructureDependency
{
    public static IServiceCollection AddInfrasctructureDependencies(this IServiceCollection services)
    {
        AddContext(services);

        AddPersistence(services);

        return services;
    }

    private static void AddContext(IServiceCollection services)
    {
        services.AddDbContext<BooKingReserveContext>();
    }

    private static void AddPersistence(IServiceCollection services)
    {
        services.AddTransient<IReservationRepository, ReservationRepository>();        
        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
    }
}
