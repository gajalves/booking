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
    }

    public static void UseConsumersRabbitMQ(this IHost host)
    {
        var bus = host.Services.GetRequiredService<IEventBus>();

        bus.Subscribe<NewUserEmailEvent, NewUserEmailEventHandler>(
                QueueMapping.BookingEmailServiceNewUser, ExchangeMapping.BookingEmailService, prefetchCount: 10, deadLetter: true);
    }
}
