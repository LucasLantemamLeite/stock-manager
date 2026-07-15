using StockManager.Api.Shared.Requests.Inputs;
using System.ComponentModel.DataAnnotations;

namespace StockManager.Api.Requests.Inputs;

public sealed record LoginUserInput : ConfirmPasswordInput
{
    [Required(ErrorMessage = "O campo 'e-mail' é obrigatório.")]
    [EmailAddress(ErrorMessage = "O campo 'e-email' não possui um campo válido.")]
    public required string Email { get; init; }
}