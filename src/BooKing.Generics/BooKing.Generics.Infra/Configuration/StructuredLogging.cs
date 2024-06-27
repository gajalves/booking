using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace BooKing.Generics.Infra.Configuration;
public static class StructuredLogging
{
    public static void AddSegAndSerilog(this ConfigureHostBuilder host)
    {
        host.UseSerilog((context, loggerConfig) =>
        loggerConfig.ReadFrom.Configuration(context.Configuration));
    }

    public static IHostBuilder AddSegAndSerilog(this IHostBuilder host)
    {
        host.UseSerilog((context, loggerConfig) =>
        loggerConfig.ReadFrom.Configuration(context.Configuration));

        return host;
    }
}
