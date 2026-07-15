using StockManager.Api.Contracts.Users.Inputs;
using System.ComponentModel.DataAnnotations;

namespace StockManager.Api.Requests.Users.Inputs;

public sealed record LoginUserInput : ConfirmPasswordInput
{
    [Required(ErrorMessage = "O campo 'e-mail' é obrigatório.")]
    [EmailAddress(ErrorMessage = "O campo 'e-email' não possui um campo válido.")]
    public required string Email { get; init; }
}