using Microsoft.EntityFrameworkCore;
using StockManager.Api.Data.Context;
using StockManager.Api.Enums;
using StockManager.Api.Interfaces;
using StockManager.Api.Models;
using StockManager.Api.Requests.Inputs;
using StockManager.Api.Requests.Outputs;
using System.Net;

namespace StockManager.Api.UseCases;

public sealed class CreateUserUseCase(AppDbContext context, IHasherService hasherService, ITokenService tokenService)
{
    public async Task<UseCaseResult<string>> ExecuteAsync(CreateUserInput requestInput)
    {
        if (await context.Users.AnyAsync(u => u.Email == requestInput.Email))
            return new(HttpStatusCode.Conflict, "Email já está em uso.", false);

        if (await context.Users.AnyAsync(u => u.Phone == requestInput.Phone))
            return new(
                HttpStatusCode: HttpStatusCode.Conflict,
                Message: "Número de telefone já está em uso.",
                StopExecution: true
            );

        var userPasswordHash = hasherService.GeneratePasswordHash(requestInput.Password);

        var userToAdd = new User(
            name: requestInput.Name,
            email: requestInput.Email,
            phone: requestInput.Phone,
            password: userPasswordHash,
            companyId: Guid.NewGuid(),
            role: (Role)requestInput.Role
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