using BooKing.Apartments.Api.Configuration;
using BooKing.Generics.Api.Configuration;
using BooKing.Apartments.Infra;
using BooKing.Apartments.Application;
using BooKing.Apartments.Infra.Context;
using BooKing.Generics.Infra;
using BooKing.Generics.Infra.Configuration;
using BooKing.Generics.Shared.CurrentUserService;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Host.AddSegAndSerilog();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiConfiguration(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddInfrasctructureDependencies();
builder.Services.AddApplicationDependencies();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddCurrentUserService();

var app = builder.Build();

app.UseStaticFiles();
app.UseSwaggerConfiguration(builder.Configuration);
app.UseApiConfiguration(builder.Environment);
app.Services.RunMigration<BooKingApartmentsContext>();

app.Run();
