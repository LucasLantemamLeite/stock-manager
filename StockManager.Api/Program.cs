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
        password: requestInput.Password,
        companyId: Guid.NewGuid(),
        role: (Role)requestInput.Role
    );

    context.Users.Add(userToAdd);

    await context.SaveChangesAsync();

    return Results.Ok(new { message = "Usuário adicionado com sucesso.", data = userToAdd });
});

app.MapPost("/v1/user/login", async ([FromBody] LoginUserInput requestInput, AppDbContext context) =>
{
    var userToLogin = await context.Users.SingleOrDefaultAsync(u => u.Email == requestInput.Email);

    if (userToLogin is null || userToLogin.Password != requestInput.Password)
        return Results.BadRequest("Credenciais incorretas.");

    return Results.Ok(new { message = "Login realizado com sucesso!", data = userToLogin });
});

app.MapPatch("/v1/user", async ([FromBody] UpdateUserInput requestInput, AppDbContext context) =>
{
    var userToUpdate = await context.Users.SingleOrDefaultAsync(u => u.Id == requestInput.Id);

    if (userToUpdate is null || userToUpdate.Password != requestInput.ConfirmPassword)
        return Results.BadRequest("Credenciais incorretas");

    userToUpdate.SetName(requestInput.NewName);
    userToUpdate.SetEmail(requestInput.NewEmail);
    userToUpdate.SetPhone(requestInput.NewPhone);
    userToUpdate.SetPassword(requestInput.NewPassword);
    userToUpdate.SetUpdateAtToNow();

    await context.SaveChangesAsync();

    return Results.Ok(new { message = "Usuário atualizado com sucesso!", data = userToUpdate });
});

app.MapDelete("/v1/user", async ([FromBody] DeleteUserInput requestInput, AppDbContext context) =>
{
    var userToDelete = await context.Users.SingleOrDefaultAsync(u => u.Id == requestInput.Id);

    if (userToDelete is null || userToDelete.Password != requestInput.ConfirmPassword)
        return Results.BadRequest("Credenciais incorretas");

    context.Users.Remove(userToDelete);

    await context.SaveChangesAsync();

    return Results.Ok(new { message = "Usuário deletado com sucesso.", data = userToDelete });
});

app.Run();