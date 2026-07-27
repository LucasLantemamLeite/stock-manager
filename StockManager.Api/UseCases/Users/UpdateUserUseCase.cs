using System.Net;
using Microsoft.EntityFrameworkCore;
using StockManager.Api.Contracts.Users.Inputs;
using StockManager.Api.Data.Context;
using StockManager.Api.Entities.Models;
using StockManager.Api.Services.Interfaces;
using StockManager.Api.UseCases.Result;

namespace StockManager.Api.UseCases.Users;

public sealed class UpdateUserUseCase(AppDbContext appDbContext, IHasherService hasherService)
{
    public async Task<UseCaseResult> ExecuteAsync(UpdateUserInput requestInput, User userToUpdate)
    {
        if (!hasherService.VerifyPasswordHash(userToUpdate.Password, requestInput.ConfirmPassword))
            return new UseCaseResult(
                HttpStatusCode.Unauthorized,
                "Credenciais incorretas."
            );

        if (await appDbContext.Users.AnyAsync(u => u.Email.Equals(requestInput.NewEmail)))
            return new UseCaseResult(
                HttpStatusCode.Conflict,
                "Email já está em uso."
            );

        if (await appDbContext.Users.AnyAsync(u => u.Phone.Equals(requestInput.NewPhone)))
            return new UseCaseResult(
                HttpStatusCode.Conflict,
                "Número de telefone já está em uso."
            );

        var newPasswordHash = requestInput.NewPassword is not null
            ? hasherService.GeneratePasswordHash(requestInput.NewPassword)
            : null;

        userToUpdate.SetName(requestInput.NewName);
        userToUpdate.SetEmail(requestInput.NewEmail);
        userToUpdate.SetPhone(requestInput.NewPhone);
        userToUpdate.SetPassword(newPasswordHash);
        userToUpdate.SetUpdateAtToNow();

        await appDbContext.SaveChangesAsync();

        return new UseCaseResult(
            HttpStatusCode.OK,
            "Conta do usuário atualizada com sucesso."
        );
    }
}