using StockManager.Api.Models;

namespace StockManager.Api.Interfaces;

public interface ITokenService
{
    string GenerateAuthToken(User user);
}