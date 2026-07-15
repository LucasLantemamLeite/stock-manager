using Microsoft.EntityFrameworkCore;
using StockManager.Api.Data.Context;
using StockManager.Api.Requests.Users.Inputs;
using StockManager.Api.Results;
using StockManager.Api.Services.Interfaces;
using System.Net;

namespace StockManager.Api.UseCases.Users;

public sealed class LoginUserUseCase(AppDbContext context, IHasherService hasherService, ITokenService tokenService)
{
    public async Task<UseCaseResult<string>> ExecuteAsync(LoginUserInput requestInput)
    {
        var userToLogin = await context.Users.SingleOrDefaultAsync(u => u.Email.Equals(requestInput.Email));

        if (userToLogin is null || !hasherService.VerifyPasswordHash(userToLogin.Password, requestInput.ConfirmPassword))
            return new(
                HttpStatusCode: HttpStatusCode.Unauthorized,
                Message: "Credênciais incorretas."
            );

        var userAuthToken = tokenService.GenerateAuthToken(userToLogin);

        return new(
            HttpStatusCode: HttpStatusCode.OK,
            Message: "Login realizado com sucesso.",
            Data: userAuthToken
        );
    }
}