using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using StockManager.Api.Entities.Enums;

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
    [Range(1, 3, ErrorMessage = "Deve estar entre os valores 1 e 3.")]
    [Description("Nível de acesso da conta do usuário.")]
    public required Role Role { get; init; }
}