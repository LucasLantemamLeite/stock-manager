using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StockManager.Api.Contracts.Users.Inputs;

public record ConfirmPasswordInput
{
    [Required(ErrorMessage = "O campo 'confirmação de senha' é obrigatório.")]
    [Description("Senha da conta do usuário.")]
    public required string ConfirmPassword { get; init; }
}