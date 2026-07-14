using Microsoft.EntityFrameworkCore;
using StockManager.Api.Data.Context;
using StockManager.Api.Extensions;
using StockManager.Api.Interfaces;
using StockManager.Api.Services;
using StockManager.Api.UseCases;

var builder = WebApplication.CreateBuilder(args);

var dbConnectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<AppDbContext>(c => c.UseSqlServer(dbConnectionString));

var secretKey = builder.Configuration.GetValue<string>("SecretKey")
    ?? throw new InvalidOperationException("SecretKey não encontrada no appsettings!");

builder.Services.AddSingleton<ITokenService>(new JwtTokenService(secretKey));
builder.Services.AddSingleton<IHasherService, BCryptHashService>();

builder.Services.AddTransient<CreateUserUseCase>();
builder.Services.AddTransient<LoginUserUseCase>();
builder.Services.AddTransient<UpdateUserUseCase>();
builder.Services.AddTransient<DeleteUserUseCase>();

var app = builder.Build();

app.AddUserEndpoints();

app.Run();