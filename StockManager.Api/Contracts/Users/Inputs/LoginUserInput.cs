using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StockManager.Api.Contracts.Users.Inputs;

public sealed record LoginUserInput : ConfirmPasswordInput
{
    [Required(ErrorMessage = "É obrigatório.")]
    [MaxLength(255, ErrorMessage = "Não deve ser superior a 255 caracteres.")]
    [EmailAddress(ErrorMessage = "Formato inválido.")]
    [Description("Email da conta do usuário.")]
    public required string Email { get; init; }
}