using System.ComponentModel;
using StockManager.Api.Entities.Users.Enums;
using System.ComponentModel.DataAnnotations;

namespace StockManager.Api.Contracts.Users.Inputs;

public sealed record CreateUserInput
{
    [Required(ErrorMessage = "É obrigatório.")]
    [MaxLength(50, ErrorMessage = "Não deve ser superior a 50 caracteres.")]
    [Description("Nome da conta do usuário.")]
    public required string Name { get; init; }

    [Required(ErrorMessage = "É obrigatório.")]
    [MaxLength(255, ErrorMessage = "Não deve ser superior a 255 caracteres.")]
    [EmailAddress(ErrorMessage = "Formato inválido.")]
    [Description("Email da conta do usuário.")]
    public required string Email { get; init; }

    [Required(ErrorMessage = "É obrigatório.")]
    [MaxLength(20, ErrorMessage = "Não deve ser superior a 20 caracteres.")]
    [Phone(ErrorMessage = "Formato inválido.")]
    [Description("Número de telefone da conta do usuário.")]
    public required string Phone { get; init; }

    [Required(ErrorMessage = "É obrigatório.")]
    [MinLength(8, ErrorMessage = "Deve ser superior a 8 caracteres.")]
    [MaxLength(30, ErrorMessage = "Não deve ser superior a 30 caracteres.")]
    [Description("Senha da conta do usuário.")]
    public required string Password { get; init; }

    [Required(ErrorMessage = "É obrigatório.")]
    [Range(1, 2, ErrorMessage = "Deve estar entre os valores 1 e 2.")]
    [Description("Nível de acesso da conta do usuário.")]
    public required Role Role { get; init; }
}