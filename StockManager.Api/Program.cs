using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using StockManager.Api.Data.Context;
using StockManager.Api.Extensions.Configurations;
using StockManager.Api.Middlewares;
using StockManager.Api.Services.Implementation;
using StockManager.Api.Services.Interfaces;
using StockManager.Api.UseCases.Users;

var builder = WebApplication.CreateBuilder(args);

var dbConnectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<AppDbContext>(c => c.UseSqlServer(dbConnectionString));

var secretKey = builder.Configuration.GetValue<string>("SecretKey")
    ?? throw new InvalidOperationException("SecretKey não encontrada no appsettings!");

builder.Services.ConfigureJwtAuthentication(secretKey);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddSingleton<ITokenService>(new JwtTokenService(secretKey));
builder.Services.AddSingleton<IHasherService, BCryptHashService>();

builder.Services.AddTransient<CreateUserUseCase>();
builder.Services.AddTransient<LoginUserUseCase>();
builder.Services.AddTransient<UpdateUserUseCase>();
builder.Services.AddTransient<DeleteUserUseCase>();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();

app.MapScalarApiReference();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();