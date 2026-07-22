using System.ComponentModel;
using StockManager.Api.Entities.Users.Enums;
using System.ComponentModel.DataAnnotations;

namespace StockManager.Api.Contracts.Users.Inputs;

public sealed record CreateUserInput
{
    [Required(ErrorMessage = "O campo 'nome' é obrigatório.")]
    [MaxLength(50, ErrorMessage = "O campo 'nome' não deve ser superior a 50 caracteres.")]
    [Description("Nome da conta do usuário.")]
    public required string Name { get; init; }

    [Required(ErrorMessage = "O campo 'e-mail' é obrigatório.")]
    [MaxLength(255, ErrorMessage = "O campo 'e-mail' não deve ser superior a 255 caracteres.")]
    [EmailAddress(ErrorMessage = "O campo 'e-email' não possui um campo válido.")]
    [Description("Email da conta do usuário.")]
    public required string Email { get; init; }

    [Required(ErrorMessage = "O campo 'número de telefone' é obrigatório.")]
    [MaxLength(20, ErrorMessage = "O campo 'número de telefone' não deve ser superior a 20 caracteres.")]
    [Phone(ErrorMessage = "O campo 'número de telefone' não possui um formato válido.")]
    [Description("Número de telefone da conta do usuário.")]
    public required string Phone { get; init; }

    [Required(ErrorMessage = "O campo 'senha' é obrigatório.")]
    [MinLength(8, ErrorMessage = "O campo 'senha' deve ser superior a 8 caracteres.")]
    [MaxLength(30, ErrorMessage = "O campo 'senha' não deve ser superior a 30 caracteres.")]
    [Description("Senha da conta do usuário.")]
    public required string Password { get; init; }

    [Required(ErrorMessage = "O campo 'nível' é obrigatório.")]
    [Range(1, 2, ErrorMessage = "O campo 'nível' deve estar entre os valores 1 e 2.")]
    [Description("Nível de acesso da conta do usuário.")]
    public required Role Role { get; init; }
}