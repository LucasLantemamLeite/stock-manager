using StockManager.Api.Entities.Models;

namespace StockManager.Api.Services.Interfaces;

public interface ITokenService
{
    string GenerateAuthToken(User user);
}