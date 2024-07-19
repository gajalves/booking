using Booking.Reserve.Infra.Context;
using Booking.Reserve.Infra.Mappings;
using BooKing.Generics.Api.Middlewares;
using BooKing.Generics.Api.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using BooKing.Generics.Shared;

namespace Booking.Reserve.Api.Configuration;

public static class ApiConfiguration
{
    public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddDbContext<BooKingReserveContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            o => o.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.ReservationsSchema)));

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAny",
                            builder =>
                                builder
                                    .AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
        });

        var externalServicesConfigurationProvider = new ExternalServices();
        configuration.Bind("ExternalServices", externalServicesConfigurationProvider);
        services.AddSingleton(externalServicesConfigurationProvider);

        services.AddDependencyInjectionApi();

        services.AddControllers();
    }

    public static void UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowAny");
        app.UseMiddleware<LoggingMiddleware>();
        app.UseAuthConfiguration();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    private static void AddDependencyInjectionApi(this IServiceCollection services)
    {

    }
}
