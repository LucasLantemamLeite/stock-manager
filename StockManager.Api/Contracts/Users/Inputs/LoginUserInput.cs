using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StockManager.Api.Contracts.Users.Inputs;

public sealed record LoginUserInput : ConfirmPasswordInput
{
    [Required(ErrorMessage = "O campo 'e-mail' é obrigatório.")]
    [EmailAddress(ErrorMessage = "O campo 'e-email' não possui um campo válido.")]
    [Description("Email da conta do usuário.")]
    public required string Email { get; init; }
}