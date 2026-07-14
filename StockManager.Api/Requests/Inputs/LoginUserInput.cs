using StockManager.Api.Shared.Requests.Inputs;
using System.ComponentModel.DataAnnotations;

namespace StockManager.Api.Requests.Inputs;

public sealed record LoginUserInput : ConfirmPasswordInput
{
    [Required(ErrorMessage = "O campo 'e-mail' é obrigatório.")]
    public required string Email { get; init; }
};