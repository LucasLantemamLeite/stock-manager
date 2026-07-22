using Microsoft.EntityFrameworkCore;
using StockManager.Api.Contracts.Users.Inputs;
using StockManager.Api.Data.Context;
using StockManager.Api.Services.Interfaces;
using StockManager.Api.UseCases.Result;
using System.Net;
using StockManager.Api.Entities.Users.Models;

namespace StockManager.Api.UseCases.Users;

public sealed class DeleteUserUseCase(AppDbContext context, IHasherService hasherService)
{
    public async Task<UseCaseResult<User>> ExecuteAsync(ConfirmPasswordInput requestInput, Guid userTargetId)
    {
        var userToDelete = await context.Users.SingleOrDefaultAsync(u => u.Id.Equals(userTargetId));

        if (userToDelete is null || !hasherService.VerifyPasswordHash(userToDelete.Password, requestInput.ConfirmPassword))
            return new UseCaseResult<User>(
                HttpStatusCode: HttpStatusCode.Unauthorized,
                Message: "Credênciais incorretas."
            );

        context.Users.Remove(userToDelete);

        await context.SaveChangesAsync();

        return new UseCaseResult<User>(
            HttpStatusCode: HttpStatusCode.OK,
            Message: "Conta deletada com sucesso!"
        );
    }
}