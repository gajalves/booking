using BooKing.Apartments.Infra.Context;
using BooKing.Generics.Api.Configuration;
using BooKing.Generics.Api.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using BooKing.Apartments.Infra.Mappings;

namespace BooKing.Apartments.Api.Configuration;

public static class ApiConfiguration
{
    public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var a = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<BooKingApartmentsContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            o => o.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.ApartmentsSchema)));
        
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
        app.UseCors("Total");
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
