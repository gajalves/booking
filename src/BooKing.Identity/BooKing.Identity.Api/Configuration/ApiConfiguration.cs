using BooKing.Generics.Api.Configuration;
using BooKing.Generics.Api.Middlewares;
using BooKing.Identity.Infra.Context;
using BooKing.Identity.Infra.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BooKing.Identity.Api.Configuration;

public static class ApiConfiguration
{
    public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {

        
        services.AddDbContext<BooKingIdentityContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DataBaseConnection"),
            o => o.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.IdentitySchema)));

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAny",
                            builder =>
                                builder
                                    .AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
        });

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
