using Microsoft.EntityFrameworkCore;
using StockManager.Api.Contracts.Users.Inputs;
using StockManager.Api.Data.Context;
using StockManager.Api.Services.Interfaces;
using StockManager.Api.UseCases.Result;
using System.Net;

namespace StockManager.Api.UseCases.Users;

public sealed class UpdateUserUseCase(AppDbContext context, IHasherService hasherService)
{
    public async Task<UseCaseResult> ExecuteAsync(UpdateUserInput requestInput, Guid userTargetId)
    {
        var userToUpdate = await context.Users.SingleOrDefaultAsync(u => u.Id.Equals(userTargetId));

        if (userToUpdate is null || !hasherService.VerifyPasswordHash(userToUpdate.Password, requestInput.ConfirmPassword))
            return new(
                HttpStatusCode: HttpStatusCode.Unauthorized,
                Message: "Credenciais incorretas."
            );

        if (await context.Users.AnyAsync(u => u.Email.Equals(requestInput.NewEmail)))
            return new(
                HttpStatusCode: HttpStatusCode.Conflict,
                Message: "Email já está em uso."
            );

        if (await context.Users.AnyAsync(u => u.Phone.Equals(requestInput.NewPhone)))
            return new(
                HttpStatusCode: HttpStatusCode.Conflict,
                Message: "Número de telefone já está em uso."
            );

        var newPasswordHash = requestInput.NewPassword is not null
            ? hasherService.GeneratePasswordHash(requestInput.NewPassword)
            : null;

        userToUpdate.SetName(requestInput.NewName);
        userToUpdate.SetEmail(requestInput.NewEmail);
        userToUpdate.SetPhone(requestInput.NewPhone);
        userToUpdate.SetPassword(newPasswordHash);
        userToUpdate.SetUpdateAtToNow();

        await context.SaveChangesAsync();

        return new(
            HttpStatusCode: HttpStatusCode.OK,
            Message: "Conta do usuário atualizada com sucesso."
        );
    }
}