using Microsoft.EntityFrameworkCore;
using StockManager.Api.Data.Context;
using StockManager.Api.Extensions;
using StockManager.Api.Interfaces;
using StockManager.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var dbConnectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<AppDbContext>(c => c.UseSqlServer(dbConnectionString));

var secretKey = builder.Configuration.GetValue<string>("SecretKey")
    ?? throw new InvalidOperationException("SecretKey não encontrada no appsettings!");

builder.Services.AddSingleton<ITokenService>(new JwtTokenService(secretKey));
builder.Services.AddSingleton<IHasherService, BCryptHashService>();

var app = builder.Build();

app.AddUserEndpoints();

app.Run();