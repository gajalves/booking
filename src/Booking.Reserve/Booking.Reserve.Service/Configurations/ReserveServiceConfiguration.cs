﻿using BooKing.Reserve.Domain.Interfaces;
using BooKing.Reserve.Infra.Context;
using BooKing.Reserve.Infra.Repositories;
using BooKing.Reserve.Service.Handlers;
using BooKing.Generics.Bus.Abstractions;
using BooKing.Generics.Bus.Exchanges;
using BooKing.Generics.Bus.Queues;
using BooKing.Generics.Infra;
using BooKing.Generics.Infra.Implementations;
using BooKing.Generics.Outbox.Events;
using Microsoft.EntityFrameworkCore;
using BooKing.Generics.Outbox.Configurations;
using Microsoft.EntityFrameworkCore.Migrations;
using BooKing.Reserve.Infra.Mappings;
using Booking.Reserve.Service.Handlers;
using Booking.Reserve.Application.Interfaces;
using Booking.Reserve.Application.Services;

namespace BooKing.Reserve.Service.Configurations;
public static class ReserveServiceConfiguration
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BooKingReserveContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            o => o.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.ReservationsSchema)),
            ServiceLifetime.Singleton);

        services.AddSingleton<IReservationRepository, ReservationRepository>();
        services.AddSingleton<IPaymentService, FakePaymentService>();
        services.AddSingleton(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

        services.AddOutboxConfigurationSingleton(configuration);
    }

    public static void AddEventHandlersDependency(this IServiceCollection services)
    {
        services.AddSingleton<ReservationConfirmedEventHandler>();
        services.AddSingleton<ReservationPaymentInitiatedEventHandler>();
    }

    public static void UseConsumersRabbitMQ(this IHost host)
    {
        var bus = host.Services.GetRequiredService<IEventBus>();

        bus.Subscribe<ReservationConfirmedByUserEvent, ReservationConfirmedEventHandler>(
                QueueMapping.BooKingReserveReservationConfirmed, ExchangeMapping.BooKingReserveService, prefetchCount: 10, deadLetter: true);

        bus.Subscribe<ReservationPaymentInitiatedEvent, ReservationPaymentInitiatedEventHandler>(
                QueueMapping.BooKingReservePaymentsInitiated, ExchangeMapping.BooKingReserveService, prefetchCount: 10, deadLetter: true);
    }
}