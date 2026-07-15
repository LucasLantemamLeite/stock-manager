using StockManager.Api.Shared.Requests.Inputs;
using System.ComponentModel.DataAnnotations;

namespace StockManager.Api.Requests.Inputs;

public sealed record UpdateUserInput : ConfirmPasswordInput
{
    [MaxLength(50, ErrorMessage = "O campo 'nome' não deve ser superior a 50 caracteres.")]
    public string? NewName { get; init; }

    [MaxLength(255, ErrorMessage = "O campo 'e-mail' não deve ser superior a 255 caracteres.")]
    [EmailAddress(ErrorMessage = "O campo 'e-email' não possui um campo válido.")]
    public string? NewEmail { get; init; }

    [MaxLength(20, ErrorMessage = "O campo 'número de telefone' não deve ser superior a 20 caracteres.")]
    [Phone(ErrorMessage = "O campo 'número de telefone' não possui um formato válido.")]
    public string? NewPhone { get; init; }

    [MinLength(8, ErrorMessage = "O campo 'senha' deve ser superior a 8 caracteres.")]
    [MaxLength(30, ErrorMessage = "O campo 'senha' não deve ser superior a 30 caracteres.")]
    public string? NewPassword { get; init; }
}