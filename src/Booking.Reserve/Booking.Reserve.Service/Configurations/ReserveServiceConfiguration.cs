using Booking.Reserve.Domain.Interfaces;
using Booking.Reserve.Infra.Context;
using Booking.Reserve.Infra.Repositories;
using Booking.Reserve.Service.Handlers;
using BooKing.Generics.Bus.Abstractions;
using BooKing.Generics.Bus.Exchanges;
using BooKing.Generics.Bus.Queues;
using BooKing.Generics.Infra;
using BooKing.Generics.Infra.Implementations;
using BooKing.Generics.Outbox.Events;
using Microsoft.EntityFrameworkCore;

namespace Booking.Reserve.Service.Configurations;
public static class ReserveServiceConfiguration
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BooKingReserveContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddSingleton<IReservationRepository, ReservationRepository>();
        services.AddSingleton(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

    }

    public static void AddEventHandlersDependency(this IServiceCollection services)
    {
        services.AddSingleton<ReservationConfirmedEventHandler>();
    }

    public static void UseConsumersRabbitMQ(this IHost host)
    {
        var bus = host.Services.GetRequiredService<IEventBus>();

        bus.Subscribe<ReservationConfirmedByUserEvent, ReservationConfirmedEventHandler>(
                QueueMapping.BookingReserveReservationConfirmed, ExchangeMapping.BookingReserveService, prefetchCount: 10, deadLetter: true);
    }
}
