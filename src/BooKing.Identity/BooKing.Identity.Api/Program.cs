using BooKing.Generics.Api.Configuration;
using BooKing.Generics.Infra;
using BooKing.Generics.Infra.Configuration;
using BooKing.Generics.Outbox.Configurations;
using BooKing.Generics.Shared.CurrentUserService;
using BooKing.Identity.Api.Configuration;
using BooKing.Identity.Application;
using BooKing.Identity.Infra;
using BooKing.Identity.Infra.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Host.AddSegAndSerilog();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiConfiguration(builder.Configuration);
builder.Services.AddOutboxConfiguration(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddInfrasctructureDependencies();
builder.Services.AddApplicationDependencies();
builder.Services.AddCurrentUserService();
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

app.UseSwaggerConfiguration(builder.Configuration);
app.UseApiConfiguration(builder.Environment);
app.Services.RunMigration<BooKingIdentityContext>();

app.Run();
