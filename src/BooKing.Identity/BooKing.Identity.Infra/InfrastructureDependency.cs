using BooKing.Generics.Infra;
using BooKing.Generics.Infra.Implementations;
using BooKing.Identity.Domain.Interfaces;
using BooKing.Identity.Infra.Context;
using BooKing.Identity.Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BooKing.Identity.Infra;
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
        services.AddDbContext<BooKingIdentityContext>();
    }

    private static void AddPersistence(IServiceCollection services)
    {
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
    }
}
