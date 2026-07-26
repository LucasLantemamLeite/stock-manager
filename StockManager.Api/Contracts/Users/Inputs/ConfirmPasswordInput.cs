using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StockManager.Api.Contracts.Users.Inputs;

public record ConfirmPasswordInput
{
    [Required(ErrorMessage = "É obrigatório.")]
    [Description("Senha da conta do usuário.")]
    public required string ConfirmPassword { get; init; }
}