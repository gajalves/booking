using BooKing.Generics.Bus.Abstractions;
using BooKing.Generics.Bus.Implementations;
using MassTransit;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BooKing.Generics.Bus.Configurations;
public static class BusConfiguration
{
    public static void AddBusConfiguration(this IServiceCollection services,
                                           IConfiguration configuration,
                                           Action<IRegistrationConfigurator>? configureConsumers = null)
    {
        services.AddSingleton<IEventBus, EventBus>();

        //Masstransit
        services.AddMassTransit(busConfigurator =>
        {            
            configureConsumers?.Invoke(busConfigurator);
            
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.UsingRabbitMq((context, config) =>
            {
                config.Host(configuration["RabbitMQ:HostName"], host =>
                {
                    host.Username(configuration["RabbitMQ:UserName"]);
                    host.Password(configuration["RabbitMQ:Password"]);
                });

                config.UseRawJsonDeserializer(isDefault: true);
                config.ConfigureEndpoints(context);
            });
        });
    }
}
