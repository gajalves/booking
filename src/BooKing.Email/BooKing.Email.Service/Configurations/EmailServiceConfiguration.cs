using BooKing.Email.Service.Handlers;
using BooKing.Generics.Bus.Abstractions;
using BooKing.Generics.Bus.Exchanges;
using BooKing.Generics.Bus.Queues;
using BooKing.Generics.Outbox.Events;

namespace BooKing.Email.Service.Configurations;
public static class EmailServiceConfiguration
{
    public static void AddEventHandlersDependency(this IServiceCollection services)
    {
        services.AddSingleton<NewUserEmailEventHandler>();
        services.AddSingleton<ReservationCreatedEmailEventHandler>();
        services.AddSingleton<ServicePaymentProcessedEmailEventHandler>();
        services.AddSingleton<ReservationCancelledEmailEventHandler>();
    }

    public static void UseConsumersRabbitMQ(this IHost host)
    {
        var bus = host.Services.GetRequiredService<IEventBus>();

        bus.Subscribe<NewUserEmailEvent, NewUserEmailEventHandler>(
                QueueMapping.BooKingEmailServiceNewUser, ExchangeMapping.BooKingEmailService, prefetchCount: 10, deadLetter: true);

        bus.Subscribe<ReservationCreatedEvent, ReservationCreatedEmailEventHandler>(
                QueueMapping.BooKingEmailServiceReservationCreated, ExchangeMapping.BooKingEmailService, prefetchCount: 10, deadLetter: true);

        bus.Subscribe<ReservationPaymentProcessedEvent, ServicePaymentProcessedEmailEventHandler>(
                QueueMapping.BooKingEmailServicePaymentProcessed, ExchangeMapping.BooKingEmailService, prefetchCount: 10, deadLetter: true);
        
        bus.Subscribe<ReservationCancelledByUserEvent, ReservationCancelledEmailEventHandler>(
                QueueMapping.BooKingReserveReservationCancelled, ExchangeMapping.BooKingEmailService, prefetchCount: 10, deadLetter: true);

    }
}
