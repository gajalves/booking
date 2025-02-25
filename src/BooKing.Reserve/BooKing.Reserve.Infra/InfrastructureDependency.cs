using BooKing.Generics.Infra.Implementations;
using BooKing.Generics.Infra;
using Microsoft.Extensions.DependencyInjection;
using BooKing.Reserve.Infra.Context;
using BooKing.Reserve.Domain.Interfaces;
using BooKing.Reserve.Infra.Repositories;

namespace BooKing.Reserve.Infra;
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
