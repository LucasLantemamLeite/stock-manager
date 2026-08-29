using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StockManager.Api.Contracts.Companies.Inputs;

public sealed record CreateCompanyInput
{
    [Required(ErrorMessage = "É obrigatório.")]
    [MaxLength(30, ErrorMessage = "Não deve ser superior a 30 caracteres.")]
    [Description("Nome da empresa.")]
    public required string CompanyName { get; init;}
    
    [Required(ErrorMessage = "É obrigatório.")]
    [MaxLength(30, ErrorMessage = "Não deve ser superior a 14 caracteres.")]
    [Description("CNPJ da empresa.")]
    public required string Cnpj { get; init; }
    
    [Required(ErrorMessage = "É obrigatório.")]
    [MaxLength(50, ErrorMessage = "Não deve ser superior a 50 caracteres.")]
    [Description("Nome da conta do dono.")]
    public required string OwnerName { get; init; }
    
    [Required(ErrorMessage = "É obrigatório.")]
    [MaxLength(255, ErrorMessage = "Não deve ser superior a 255 caracteres.")]
    [EmailAddress(ErrorMessage = "Formato inválido.")]
    [Description("Email da conta do dono.")]
    public required string Email { get; init; }
    
    [Required(ErrorMessage = "É obrigatório.")]
    [MaxLength(20, ErrorMessage = "Não deve ser superior a 20 caracteres.")]
    [Phone(ErrorMessage = "Formato inválido.")]
    [Description("Número de telefone da conta do dono.")]
    public required string Phone { get; init; }
    
    [Required(ErrorMessage = "É obrigatório.")]
    [MinLength(8, ErrorMessage = "Deve ser superior a 8 caracteres.")]
    [MaxLength(30, ErrorMessage = "Não deve ser superior a 30 caracteres.")]
    [Description("Senha da conta do dono.")]
    public required string Password { get; init; }
};