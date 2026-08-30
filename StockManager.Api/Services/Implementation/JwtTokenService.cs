using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using StockManager.Api.Entities.Models;
using StockManager.Api.Services.Interfaces;

namespace StockManager.Api.Services.Implementation;

public sealed class JwtTokenService(string secretKey) : ITokenService
{
    private string SecretKey { get; } = secretKey;

    public string GenerateAuthToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var encodedKey = Encoding.UTF8.GetBytes(SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(encodedKey),
                SecurityAlgorithms.HmacSha256),
            Expires = DateTime.UtcNow.AddHours(4),
            Issuer = "stock-manager-server",
            Audience = "stock-manager-client",
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("companyid", user.CompanyId.ToString()),
                new Claim(ClaimTypes.Role, ((int)user.Role).ToString())
            ])
        };

        var userAuthToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(userAuthToken);
    }
}