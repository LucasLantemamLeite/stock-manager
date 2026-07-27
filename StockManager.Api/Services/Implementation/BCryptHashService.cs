using StockManager.Api.Services.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace StockManager.Api.Services.Implementation;

public sealed class BCryptHashService : IHasherService
{
    public string GeneratePasswordHash(string passwordTarget)
    {
        return BC.HashPassword(passwordTarget);
    }

    public bool VerifyPasswordHash(string passwordHash, string passwordTarget)
    {
        return BC.Verify(passwordTarget, passwordHash);
    }
}