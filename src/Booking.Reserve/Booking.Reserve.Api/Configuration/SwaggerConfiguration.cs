using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Booking.Reserve.Api.Configuration;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "BooKing.Reserve.Api",
                Description = "",
                Contact = new OpenApiContact() { Name = "Gabriel Jaime", Email = "ga.jaimealves@gmail.com" },
                License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Insert your JWT Token: Bearer {your token}",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app, IConfiguration configuration)
    {
        var basePath = configuration["JwtSettings:BasePath"];

        app.UseSwagger(c =>
        {
            c.RouteTemplate = "swagger/{documentName}/swagger.json";
            c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
            {
                swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{basePath}" } };
            });
        });
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint($"{basePath}/swagger/v1/swagger.json", "API V1");
            c.InjectStylesheet($"{basePath}/swagger-ui/custom.css");
            c.DocExpansion(DocExpansion.None);
            c.OAuthClientId("swagger-ui");
            c.OAuthClientSecret("swagger-ui-secret");
            c.OAuthRealm("swagger-ui-realm");
            c.OAuthAppName("Swagger UI");
        });

        return app;
    }
}

