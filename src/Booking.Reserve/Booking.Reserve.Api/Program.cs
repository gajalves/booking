using Booking.Reserve.Api.Configuration;
using Booking.Reserve.Application;
using Booking.Reserve.Infra;
using Booking.Reserve.Infra.Context;
using BooKing.Generics.Api.Configuration;
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

app.UseSwaggerConfiguration(builder.Configuration);
app.UseApiConfiguration(builder.Environment);
app.Services.RunMigration<BooKingReserveContext>();

app.Run();