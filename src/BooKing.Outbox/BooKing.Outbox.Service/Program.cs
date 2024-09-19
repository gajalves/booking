using Autofac.Extensions.DependencyInjection;
using BooKing.Generics.Bus.Configurations;
using BooKing.Generics.Infra;
using BooKing.Generics.Infra.Configuration;
using BooKing.Generics.Outbox.Configurations;
using BooKing.Generics.Outbox.Context;
using BooKing.Outbox.Service.Executors;

IHost host = Host.CreateDefaultBuilder(args)
    .AddSegAndSerilog()
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<OutboxEventsExecutor>();
        services.AddBusConfiguration(hostContext.Configuration);
        services.Configure<OutboxOptions>(hostContext.Configuration.GetSection("OutboxOptions"));
        services.AddOutboxConfigurationSingleton(hostContext.Configuration);
    })    
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .Build();

host.Services.RunMigration<OutboxContext>();

host.Run();
