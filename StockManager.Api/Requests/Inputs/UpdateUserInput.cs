namespace StockManager.Api.Requests.Inputs;

public sealed record UpdateUserInput(
    string? NewName,
    string? NewEmail,
    string? NewPhone,
    string? NewPassword,
    Guid Id,
    string ConfirmPassword
);