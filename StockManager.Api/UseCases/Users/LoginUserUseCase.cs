using System.Net;
using Microsoft.EntityFrameworkCore;
using StockManager.Api.Contracts.Users.Inputs;
using StockManager.Api.Data.Context;
using StockManager.Api.Services.Interfaces;
using StockManager.Api.UseCases.Result;

namespace StockManager.Api.UseCases.Users;

public sealed class LoginUserUseCase(
    AppDbContext appDbContext,
    IHasherService hasherService,
    ITokenService tokenService)
{
    public async Task<UseCaseResult<string>> ExecuteAsync(LoginUserInput requestInput)
    {
        var userToLogin = await appDbContext.Users.SingleOrDefaultAsync(u => u.Email.Equals(requestInput.Email));
        
        if (userToLogin is null
            || !hasherService.VerifyPasswordHash(userToLogin.Password, requestInput.ConfirmPassword))
            return new UseCaseResult<string>(
                HttpStatusCode.Unauthorized,
                "Credênciais incorretas."
            );

        if (userToLogin.PasswordMustBeChanged)
            return new UseCaseResult<string>(
                HttpStatusCode.OK,
                "Nova conta identificada. Senha temporária deve ser alterada."
            );
        
        var userAuthToken = tokenService.GenerateAuthToken(userToLogin);

        return new UseCaseResult<string>(
            HttpStatusCode.OK,
            "Login realizado com sucesso.",
            userAuthToken
        );
    }
}