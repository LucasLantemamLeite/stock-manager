namespace StockManager.Api.Interfaces;

public interface IHasherService
{
    string GeneratePasswordHash(string passwordTarget);

    bool VerifyPasswordHash(string passwordHash, string passwordTarget);
}