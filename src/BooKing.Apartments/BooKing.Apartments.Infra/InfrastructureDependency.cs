using BooKing.Generics.Infra.Implementations;
using BooKing.Generics.Infra;
using Microsoft.Extensions.DependencyInjection;
using BooKing.Apartments.Infra.Context;
using BooKing.Apartments.Domain.Interfaces;
using BooKing.Apartments.Infra.Repositories;

namespace BooKing.Apartments.Infra;
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
        services.AddDbContext<BooKingApartmentsContext>();
    }

    private static void AddPersistence(IServiceCollection services)
    {
        services.AddTransient<IApartmentRepository, ApartmentRepository>();
        services.AddTransient<IAmenityRepository, AmenityRepository>();
        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
    }
}
