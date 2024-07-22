using Autofac.Extensions.DependencyInjection;
using BooKing.Reserve.Service;
using BooKing.Reserve.Service.Configurations;
using BooKing.Generics.Bus.Configurations;
using BooKing.Generics.Infra.Configuration;

IHost host = Host.CreateDefaultBuilder(args)
    .AddSegAndSerilog()
    .ConfigureServices((hostContext, services) =>
    {
        services.AddBusConfiguration(hostContext.Configuration);
        services.AddDependencies(hostContext.Configuration);
        services.AddEventHandlersDependency();
        services.AddHostedService<Worker>();

    })
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .Build();

host.UseConsumersRabbitMQ();

host.Run();