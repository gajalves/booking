using BooKing.Generics.Infra;
using BooKing.Generics.Infra.Implementations;
using BooKing.Generics.Outbox.Configurations;
using BooKing.Reserve.Application.Interfaces;
using BooKing.Reserve.Application.Services;
using BooKing.Reserve.Domain.Interfaces;
using BooKing.Reserve.Infra.Context;
using BooKing.Reserve.Infra.Mappings;
using BooKing.Reserve.Infra.Repositories;
using BooKing.Reserve.Service.Handlers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BooKing.Reserve.Service.Configurations;
public static class ReserveServiceConfiguration
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BooKingReserveContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DataBaseConnection"),
            o => o.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.ReservationsSchema)),
            ServiceLifetime.Singleton);

        services.AddSingleton<IReservationRepository, ReservationRepository>();
        services.AddSingleton<IPaymentService, FakePaymentService>();
        services.AddSingleton(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

        services.AddOutboxConfigurationSingleton(configuration);

        services.Configure<ExecutorOptions>(configuration.GetSection("ExecutorOptions"));
    }

    public static Action<IRegistrationConfigurator> AddConsumers =>
    configurator =>
    {
        configurator.AddConsumer<ReservationConfirmedEventHandler>();
        configurator.AddConsumer<ReservationPaymentInitiatedEventHandler>();
        configurator.AddConsumer<ReservationReservedEventHandler>();
    };
}
