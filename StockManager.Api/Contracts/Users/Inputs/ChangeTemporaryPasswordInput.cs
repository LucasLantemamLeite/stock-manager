using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StockManager.Api.Contracts.Users.Inputs;

public sealed record ChangeTemporaryPasswordInput : ConfirmPasswordInput
{
    [Required(ErrorMessage = "É obrigatório.")]
    [MaxLength(255, ErrorMessage = "Não deve ser superior a 255 caracteres.")]
    [EmailAddress(ErrorMessage = "Formato inválido.")]
    [Description("Email da conta do usuário.")]
    public required string Email { get; init; }
    
    [Required(ErrorMessage = "É obrigatório.")]
    [Description("Nova senha da conta do usuário.")]
    public required string NewPassword { get; init; }
};