using Microsoft.EntityFrameworkCore;
using StockManager.Api.Data.Context;
using StockManager.Api.Interfaces;
using StockManager.Api.Requests.Inputs;
using StockManager.Api.Requests.Outputs;
using System.Net;

namespace StockManager.Api.UseCases;

public sealed class LoginUserUseCase(AppDbContext context, IHasherService hasherService, ITokenService tokenService)
{
    public async Task<UseCaseResult<string>> ExecuteAsync(LoginUserInput requestInput)
    {
        var userToLogin = await context.Users.SingleOrDefaultAsync(u => u.Email == requestInput.Email);

        if (userToLogin is null || !hasherService.VerifyPasswordHash(userToLogin.Password, requestInput.ConfirmPassword))
            return new(HttpStatusCode.Unauthorized, "Credênciais incorretas.", false);

        var userAuthToken = tokenService.GenerateAuthToken(userToLogin);

        return new(HttpStatusCode.OK, "Login realizado com sucesso.", true, userAuthToken);
    }
}