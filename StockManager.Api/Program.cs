using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManager.Api.Data.Context;
using StockManager.Api.Enums;
using StockManager.Api.Models;
using StockManager.Api.Requests.Inputs;

var builder = WebApplication.CreateBuilder(args);

var dbConnectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<AppDbContext>(c => c.UseSqlServer(dbConnectionString));

var app = builder.Build();

app.MapPost("/v1/user", async ([FromBody] CreateUserInput requestInput, AppDbContext context) =>
{
    if (await context.Users.AnyAsync(u => u.Email == requestInput.Email))
        return Results.Conflict("Email já está em uso.");

    if (await context.Users.AnyAsync(u => u.Phone == requestInput.Phone))
        return Results.Conflict("Número de telefone já está em uso.");

    var userToAdd = new User(
        name: requestInput.Name,
        email: requestInput.Email,
        phone: requestInput.Phone,
        companyId: Guid.NewGuid(),
        role: (Role)requestInput.Role
    );

    context.Users.Add(userToAdd);

    await context.SaveChangesAsync();

    return Results.Ok(new { message = "Usuário adicionado com sucesso.", data = userToAdd });
});

app.Run();