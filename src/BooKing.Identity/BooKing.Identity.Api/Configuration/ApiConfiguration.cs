using BooKing.Generics.Api.Configuration;
using BooKing.Identity.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace BooKing.Identity.Api.Configuration;

public static class ApiConfiguration
{
    public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BooKingIdentityContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

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
