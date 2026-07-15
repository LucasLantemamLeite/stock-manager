using StockManager.Api.Entities.Users.Models;

namespace StockManager.Api.Services.Interfaces;

public interface ITokenService
{
    string GenerateAuthToken(User user);
}