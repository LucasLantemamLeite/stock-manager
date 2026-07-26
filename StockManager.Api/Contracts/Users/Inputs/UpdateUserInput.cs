using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StockManager.Api.Contracts.Users.Inputs;

public sealed record UpdateUserInput : ConfirmPasswordInput
{
    [MaxLength(50, ErrorMessage = "Não deve ser superior a 50 caracteres.")]
    [Description("Novo nome da conta do usuário.")]
    public string? NewName { get; init; }

    [MaxLength(255, ErrorMessage = "Não deve ser superior a 255 caracteres.")]
    [EmailAddress(ErrorMessage = "Formato inválido.")]
    [Description("Novo email da conta do usuário.")]
    public string? NewEmail { get; init; }

    [MaxLength(20, ErrorMessage = "Não deve ser superior a 20 caracteres.")]
    [Phone(ErrorMessage = "Formato inválido.")]
    [Description("Novo número de telefone da conta do usuário.")]
    public string? NewPhone { get; init; }

    [MinLength(8, ErrorMessage = "Deve ser superior a 8 caracteres.")]
    [MaxLength(30, ErrorMessage = "Não deve ser superior a 30 caracteres.")]
    [Description("Nova senha da conta do usuário.")]
    public string? NewPassword { get; init; }
}