using StockManager.Api.Contracts.Users.Inputs;
using StockManager.Api.Data.Context;
using StockManager.Api.Services.Interfaces;
using StockManager.Api.UseCases.Result;
using System.Net;
using StockManager.Api.Entities.Users.Models;

namespace StockManager.Api.UseCases.Users;

public sealed class DeleteUserUseCase(AppDbContext appDbContext, IHasherService hasherService)
{
    public async Task<UseCaseResult> ExecuteAsync(ConfirmPasswordInput requestInput, User userToDelete)
    {
        if (!hasherService.VerifyPasswordHash(userToDelete.Password, requestInput.ConfirmPassword))
            return new UseCaseResult(
                HttpStatusCode: HttpStatusCode.Unauthorized,
                Message: "Credênciais incorretas."
            );

        appDbContext.Users.Remove(userToDelete);

        await appDbContext.SaveChangesAsync();

        return new UseCaseResult(
            HttpStatusCode: HttpStatusCode.OK,
            Message: "Conta deletada com sucesso!"
        );
    }
}