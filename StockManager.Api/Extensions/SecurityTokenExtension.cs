using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace StockManager.Api.Extensions;

public static class SecurityTokenExtension
{
    public static void AddAuthenticationConfiguration(this IServiceCollection services, string secretKey)
    {
        services.AddAuthentication(options =>
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

        services.AddAuthorization();
    }
}