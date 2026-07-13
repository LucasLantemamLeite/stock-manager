using Microsoft.IdentityModel.Tokens;
using StockManager.Api.Interfaces;
using StockManager.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StockManager.Api.Services;

public sealed class JwtTokenService(string secretKey) : ITokenService
{
    public string SecretKey { get; } = secretKey;

    public string GenerateAuthToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var encodedKey = Encoding.UTF8.GetBytes(SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor()
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
                new Claim(ClaimTypes.Role, user.Role.ToString())
            ]),
        };

        var userAuthToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(userAuthToken);
    }
}