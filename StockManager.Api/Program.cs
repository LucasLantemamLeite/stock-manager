using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StockManager.Api.Data.Context;
using StockManager.Api.Interfaces;
using StockManager.Api.Services;
using StockManager.Api.UseCases;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var dbConnectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<AppDbContext>(c => c.UseSqlServer(dbConnectionString));

var secretKey = builder.Configuration.GetValue<string>("SecretKey")
    ?? throw new InvalidOperationException("SecretKey não encontrada no appsettings!");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => options.TokenValidationParameters = new()
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
    ValidTypes = ["JWT"],
    ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
    ValidateIssuer = true,
    ValidIssuer = "stock-manager-server",
    RequireAudience = true,
    ValidAudience = "stock-manager-client",
    ValidateLifetime = true,
    RequireExpirationTime = true,
});

builder.Services
    .AddAuthorization()
    .Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddControllers();

builder.Services.AddSingleton<ITokenService>(new JwtTokenService(secretKey));
builder.Services.AddSingleton<IHasherService, BCryptHashService>();

builder.Services.AddTransient<CreateUserUseCase>();
builder.Services.AddTransient<LoginUserUseCase>();
builder.Services.AddTransient<UpdateUserUseCase>();
builder.Services.AddTransient<DeleteUserUseCase>();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }

    catch (Exception ex)
    {
        logger.LogWarning(ex, $"Uma exceção foi capturada.");

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var errorMessage = new { message = "Ocorreu um erro interno no servidor. Tente novamente mais tarde." };

        await context.Response.WriteAsJsonAsync(errorMessage);
    }
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();