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
    IEmailService emailService)
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

        if (createUserInput.Role >= authenticatedUser.Role)
            return new UseCaseResult(
                HttpStatusCode.Forbidden,
                $"Só é permitido criar contas com acesso inferior." +
                $" Seu acesso atual: {authenticatedUser.Role} ({(int)authenticatedUser.Role})"
            );

        ICollection<char> randomPasswordChars = [];

        while (randomPasswordChars.Count < 30)
        {
            var upperCase = Random.Shared.Next(0, 2) == 0;

            var randomNumberInterval = upperCase
                ? Random.Shared.Next(65, 91)
                : Random.Shared.Next(97, 123);

            randomPasswordChars.Add((char)randomNumberInterval);
        }

        var randomPasswordGen = string.Join("", randomPasswordChars);

        var temporaryUserPasswordHash = hasherService.GeneratePasswordHash(randomPasswordGen);

        var userToAdd = new User(
            createUserInput.Name,
            createUserInput.Email,
            createUserInput.Phone,
            temporaryUserPasswordHash,
            authenticatedUser.CompanyId,
            createUserInput.Role
        );

        appDbContext.Users.Add(userToAdd);

        await appDbContext.SaveChangesAsync();

        var emailHtmlContent =
            "<div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; padding: 30px; color: #333;'>"
            + "<h1 style='color: #2563eb;'>Conta criada com sucesso! 🎉</h1>"
            + "<p>Olá! Sua conta foi criada e já está pronta para uso.</p>"
            + "<p>Utilize a senha temporária abaixo para fazer seu primeiro login. "
            + "Por segurança, altere-a assim que acessar sua conta.</p>"
            + $"<div style='background: #f3f4f6; padding: 15px; border-radius: 8px; text-align: center; font-size: 20px; font-weight: bold; letter-spacing: 2px;'>{randomPasswordGen}</div>"
            + "<p style='margin-top: 25px; color: #666; font-size: 13px;'>"
            + "Se você não solicitou esta conta, entre em contato com o suporte."
            + "</p>"
            + "<hr style='border: none; border-top: 1px solid #ddd; margin: 30px 0;'>"
            + "<p style='text-align: center; color: #999; font-size: 12px;'>StockManager</p>"
            + "</div>";

        await emailService.Send("Conta criada", emailHtmlContent);

        return new UseCaseResult(
            HttpStatusCode.Created,
            "Usuário criado com sucesso."
        );
    }
}