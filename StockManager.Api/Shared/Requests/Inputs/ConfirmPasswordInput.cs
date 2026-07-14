using System.ComponentModel.DataAnnotations;

namespace StockManager.Api.Shared.Requests.Inputs;

public record ConfirmPasswordInput
{
    [Required(ErrorMessage = "O campo 'confirmação de senha' é obrigatório.")]
    public required string ConfirmPassword { get; init; }
}