using System.Net;
using Microsoft.EntityFrameworkCore;
using StockManager.Api.Contracts.Users.Inputs;
using StockManager.Api.Data.Context;
using StockManager.Api.Entities.Models;
using StockManager.Api.Services.Interfaces;
using StockManager.Api.UseCases.Result;

namespace StockManager.Api.UseCases.Users;

public sealed class CreateUserUseCase(
    AppDbContext appDbContext,
    IHasherService hasherService,
    ITokenService tokenService)
{
    public async Task<UseCaseResult<string>> ExecuteAsync(CreateUserInput requestInput)
    {
        if (await appDbContext.Users.AnyAsync(u => u.Email.Equals(requestInput.Email)))
            return new UseCaseResult<string>(
                HttpStatusCode.Conflict,
                "Email já está em uso."
            );

        if (await appDbContext.Users.AnyAsync(u => u.Phone.Equals(requestInput.Phone)))
            return new UseCaseResult<string>(
                HttpStatusCode.Conflict,
                "Número de telefone já está em uso."
            );

        var userPasswordHash = hasherService.GeneratePasswordHash(requestInput.Password);

        var userToAdd = new User(
            requestInput.Name,
            requestInput.Email,
            requestInput.Phone,
            userPasswordHash,
            Guid.NewGuid(),
            requestInput.Role
        );

        appDbContext.Users.Add(userToAdd);

        await appDbContext.SaveChangesAsync();

        var userAuthToken = tokenService.GenerateAuthToken(userToAdd);

        return new UseCaseResult<string>(
            HttpStatusCode.Created,
            "Conta do usuário criado com sucesso.",
            userAuthToken
        );
    }
}