using BooKing.Generics.Outbox.Configurations;
using BooKing.Outbox.Service.Executors;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<OutboxEventsExecutor>()
        .AddOutboxConfigurationSingleton(hostContext.Configuration);
    })
    .Build();

host.Run();
