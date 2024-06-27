using Autofac.Extensions.DependencyInjection;
using BooKing.Email.Service.Configurations;
using BooKing.Email.Service.Services;
using BooKing.Generics.Bus.Configurations;
using BooKing.Generics.Infra.Configuration;

IHost host = Host.CreateDefaultBuilder(args)
    .AddSegAndSerilog()
    .ConfigureServices((hostContext, services) =>
    {
        services.AddBusConfiguration(hostContext.Configuration);
        services.AddEventHandlersDependency();
        services.Configure<EmailServiceOptions>(hostContext.Configuration.GetSection("EmailService"));
        services.AddSingleton<ISendEmailService, SendEmailService>();
    })
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .Build();

host.UseConsumersRabbitMQ();

host.Run();
