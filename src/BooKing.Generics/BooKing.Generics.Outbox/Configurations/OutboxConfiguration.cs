using BooKing.Generics.Infra.Implementations;
using BooKing.Generics.Infra;
using BooKing.Generics.Outbox.Context;
using BooKing.Generics.Outbox.Repository;
using BooKing.Generics.Outbox.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BooKing.Generics.Outbox.Configurations;
public static class OutboxConfiguration
{
    public static void AddOutboxConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddOutboxDependencies(services);
    }

    /*
     * Only for workers!
     */
    public static void AddOutboxConfigurationSingleton(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IOutboxEventService, OutboxEventService>();
        services.AddSingleton<IOutboxReposity, OutboxReposity>();
        services.AddSingleton<IUnitOfWork<OutboxContext>, UnitOfWork<OutboxContext>>();
        services.AddDbContext<OutboxContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            o => o.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "Outbox")),
            ServiceLifetime.Singleton);
    }

    private static void AddOutboxDependencies(IServiceCollection services)
    {
        services.AddScoped<IOutboxReposity, OutboxReposity>();
        services.AddScoped<IOutboxEventService, OutboxEventService>();        
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {        
        services.AddDbContext<OutboxContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }
}
