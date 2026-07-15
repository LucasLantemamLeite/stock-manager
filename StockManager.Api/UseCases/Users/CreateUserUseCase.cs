using Microsoft.EntityFrameworkCore;
using StockManager.Api.Data.Context;
using StockManager.Api.Entities.Users.Models;
using StockManager.Api.Requests.Users.Inputs;
using StockManager.Api.Results;
using StockManager.Api.Services.Interfaces;
using System.Net;

namespace StockManager.Api.UseCases.Users;

public sealed class CreateUserUseCase(AppDbContext context, IHasherService hasherService, ITokenService tokenService)
{
    public async Task<UseCaseResult<string>> ExecuteAsync(CreateUserInput requestInput)
    {
        if (await context.Users.AnyAsync(u => u.Email.Equals(requestInput.Email)))
            return new(
                HttpStatusCode: HttpStatusCode.Conflict,
                Message: "Email já está em uso."
            );

        if (await context.Users.AnyAsync(u => u.Phone.Equals(requestInput.Phone)))
            return new(
                HttpStatusCode: HttpStatusCode.Conflict,
                Message: "Número de telefone já está em uso."
            );

        var userPasswordHash = hasherService.GeneratePasswordHash(requestInput.Password);

        var userToAdd = new User(
            name: requestInput.Name,
            email: requestInput.Email,
            phone: requestInput.Phone,
            password: userPasswordHash,
            companyId: Guid.NewGuid(),
            role: requestInput.Role
        );

        context.Users.Add(userToAdd);

        await context.SaveChangesAsync();

        var userAuthToken = tokenService.GenerateAuthToken(userToAdd);

        return new(
            HttpStatusCode: HttpStatusCode.Created,
            Message: "Conta do usuário criado com sucesso.",
            Data: userAuthToken
        );
    }
}