using StockManager.Api.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace StockManager.Api.Services;

public sealed class BCryptHashService : IHasherService
{
    public string GeneratePasswordHash(string passwordTarget)
        => BC.HashPassword(passwordTarget);

    public bool VerifyPasswordHash(string passwordHash, string passwordTarget)
        => BC.Verify(passwordTarget, passwordHash);
}