using System.Net;
using Microsoft.EntityFrameworkCore;
using StockManager.Api.Contracts.Users.Inputs;
using StockManager.Api.Data.Context;
using StockManager.Api.Services.Interfaces;
using StockManager.Api.UseCases.Result;

namespace StockManager.Api.UseCases.Users;

public sealed class ChangeTemporaryPasswordUserUseCase(
    AppDbContext appDbContext,
    IHasherService hasherService,
    ITokenService tokenServiceo)
{
    public async Task<UseCaseResult<string>> ExecuteAsync(ChangeTemporaryPasswordInput changeTemporaryPasswordInput)
    {
        var userToChangePassword =
            await appDbContext.Users.SingleOrDefaultAsync(u => u.Email.Equals(changeTemporaryPasswordInput.Email));

        if (userToChangePassword is null || !hasherService.VerifyPasswordHash(userToChangePassword.Password,
                changeTemporaryPasswordInput.ConfirmPassword))
            return new UseCaseResult<string>(
                HttpStatusCode.Unauthorized,
                "Credenciais incorretas."
            );

        var newPasswordHash = hasherService.GeneratePasswordHash(changeTemporaryPasswordInput.NewPassword);
        
        userToChangePassword.SetPassword(newPasswordHash);
        userToChangePassword.SetPasswordMustBeChangedToFalse();

        await appDbContext.SaveChangesAsync();

        var userAuthToken = tokenServiceo.GenerateAuthToken(userToChangePassword);

        return new UseCaseResult<string>(
            HttpStatusCode.OK,
            "Senha alterada com sucesso.",
            userAuthToken
        );
    }
}