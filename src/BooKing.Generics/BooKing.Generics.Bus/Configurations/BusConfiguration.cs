using Autofac;
using BooKing.Generics.Bus.Abstractions;
using BooKing.Generics.Bus.Implementations;
using BooKing.Generics.Bus.InMemory;
using BooKing.Generics.Bus.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace BooKing.Generics.Bus.Configurations;
public static class BusConfiguration
{
    public static void AddBusConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        //EventBus
        services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        services.AddSingleton<IEventBus, EventBus>(sp =>
        {
            var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
            var rabbitMQConnection = sp.GetRequiredService<IRabbitMQConnection>();
            var logger = sp.GetRequiredService<ILogger<EventBus>>();
            var lifetimeScope = sp.GetRequiredService<ILifetimeScope>();

            return new EventBus(eventBusSubcriptionsManager, rabbitMQConnection, logger, lifetimeScope);
        });

        //RabbitMQ
        services.AddSingleton<IRabbitMQConnection>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<RabbitMQConnection>>();
            var connectionFactory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:HostName"],
                Port = int.Parse(configuration["RabbitMQ:Port"]),
                UserName = configuration["RabbitMQ:UserName"],
                Password = configuration["RabbitMQ:Password"],
                DispatchConsumersAsync = false,
                ConsumerDispatchConcurrency = 1,
            };

            return new RabbitMQConnection(connectionFactory, logger);
        });
    }
}
