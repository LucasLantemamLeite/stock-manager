using System.Net;
using Microsoft.EntityFrameworkCore;
using StockManager.Api.Contracts.Companies.Inputs;
using StockManager.Api.Data.Context;
using StockManager.Api.Entities.Enums;
using StockManager.Api.Entities.Models;
using StockManager.Api.Services.Interfaces;
using StockManager.Api.UseCases.Result;

namespace StockManager.Api.UseCases.Companies;

public sealed class CreateCompanyUseCase(AppDbContext appDbContext, ITokenService tokenService, IHasherService hasherService)
{
    public async Task<UseCaseResult<string>> ExecuteAsync(CreateCompanyInput createCompanyInput)
    {
        if (await appDbContext.Companies.AnyAsync(c => c.Cnpj.Equals(createCompanyInput.Cnpj)))
            return new UseCaseResult<string>(
                HttpStatusCode.Conflict,
                "CNPJ já está em uso."
            );
        
        if (await appDbContext.Users.AnyAsync(u => u.Email.Equals(createCompanyInput.Email)))
            return new UseCaseResult<string>(
                HttpStatusCode.Conflict,
                "Email já está em uso."
            );

        if (await appDbContext.Users.AnyAsync(u => u.Phone.Equals(createCompanyInput.Phone)))
            return new UseCaseResult<string>(
                HttpStatusCode.Conflict,
                "Número de telefone já está em uso."
            );

        var companyToAdd = new Company(
            createCompanyInput.CompanyName,
            createCompanyInput.Cnpj
        );

        var ownerPasswordHash = hasherService.GeneratePasswordHash(createCompanyInput.Password);
        
        var ownerToAdd = new User(
            createCompanyInput.OwnerName,
            createCompanyInput.Email,
            createCompanyInput.Phone,
            ownerPasswordHash,
            companyToAdd.Id,
            role: Role.Owner
        );
        
        companyToAdd.SetOwnerId(ownerToAdd.Id);

        appDbContext.AddRange(companyToAdd, ownerToAdd);
        
        await appDbContext.SaveChangesAsync();

        var ownerAuthToken = tokenService.GenerateAuthToken(ownerToAdd);

        return new UseCaseResult<string>(
            HttpStatusCode.Created,
            "Empresa criada com sucesso",
            ownerAuthToken
        );
    }
}