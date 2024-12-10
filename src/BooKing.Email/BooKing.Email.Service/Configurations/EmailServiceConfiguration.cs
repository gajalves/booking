using BooKing.Email.Service.Handlers;
using MassTransit;

namespace BooKing.Email.Service.Configurations;
public static class EmailServiceConfiguration
{    
    public static Action<IRegistrationConfigurator> AddConsumers =>
    configurator =>
    {
        configurator.AddConsumer<NewUserEmailEventHandler>();
        configurator.AddConsumer<ReservationCreatedEmailEventHandler>();
        configurator.AddConsumer<ServicePaymentProcessedEmailEventHandler>();
        configurator.AddConsumer<ReservationCancelledEmailEventHandler>();
    };
}
