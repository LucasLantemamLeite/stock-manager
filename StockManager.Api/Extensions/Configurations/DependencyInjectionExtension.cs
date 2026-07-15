using Microsoft.EntityFrameworkCore;
using StockManager.Api.Data.Context;
using StockManager.Api.Services.Implementation;
using StockManager.Api.Services.Interfaces;
using StockManager.Api.UseCases.Users;

namespace StockManager.Api.Extensions.Configurations;

public static class DependencyInjectionExtension
{
    public static void ResolveDependency(this WebApplicationBuilder builder, string secretKey)
    {
        var dbConnectionString = builder.Configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("ConnectionString do banco de dados não encontrada no appsettings.");

        builder.Services.AddDbContext<AppDbContext>(c => c.UseSqlServer(dbConnectionString));

        builder.Services.AddSingleton<ITokenService>(new JwtTokenService(secretKey));
        builder.Services.AddSingleton<IHasherService, BCryptHashService>();

        builder.Services.AddTransient<CreateUserUseCase>();
        builder.Services.AddTransient<LoginUserUseCase>();
        builder.Services.AddTransient<UpdateUserUseCase>();
        builder.Services.AddTransient<DeleteUserUseCase>();
    }
}