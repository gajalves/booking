using Autofac.Extensions.DependencyInjection;
using BooKing.Generics.Bus.Configurations;
using BooKing.Generics.Infra.Configuration;
using BooKing.Reserve.Service.Configurations;
using BooKing.Reserve.Service.Executors;

IHost host = Host.CreateDefaultBuilder(args)
    .AddSegAndSerilog()
    .ConfigureServices((hostContext, services) =>
    {
        services.AddBusConfiguration(hostContext.Configuration, ReserveServiceConfiguration.AddConsumers);
        services.AddDependencies(hostContext.Configuration);
        services.AddHostedService<ReservationCompletionExecutor>();
    })
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .Build();

host.Run();
