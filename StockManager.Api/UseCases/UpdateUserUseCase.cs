using Microsoft.EntityFrameworkCore;
using StockManager.Api.Data.Context;
using StockManager.Api.Interfaces;
using StockManager.Api.Requests.Inputs;
using StockManager.Api.Requests.Outputs;
using System.Net;

namespace StockManager.Api.UseCases;

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