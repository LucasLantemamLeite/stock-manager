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
    IHasherService hasherService)
{
    public async Task<UseCaseResult> ExecuteAsync(CreateUserInput createUserInput, User authenticatedUser)
    {
        if (await appDbContext.Users.AnyAsync(u => u.Email.Equals(createUserInput.Email)))
            return new UseCaseResult(
                HttpStatusCode.Conflict,
                "Email já está em uso."
            );
        
        if (await appDbContext.Users.AnyAsync(u => u.Phone.Equals(createUserInput.Phone)))
            return new UseCaseResult(
                HttpStatusCode.Conflict,
                "Número de telefone já está em uso."
            );

        var userPasswordHash = hasherService.GeneratePasswordHash(createUserInput.Password);
        
        var userToAdd = new User(
            createUserInput.Name,
            createUserInput.Email,
            createUserInput.Phone,
            userPasswordHash,
            authenticatedUser.CompanyId,
            createUserInput.Role
        );
        
        appDbContext.Users.Add(userToAdd);

        await appDbContext.SaveChangesAsync();

        return new UseCaseResult(
            HttpStatusCode.Created,
            "Usuário criado com sucesso."
        );
    }
}