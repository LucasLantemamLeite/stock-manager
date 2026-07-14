namespace StockManager.Api.Requests.Inputs;

public sealed record UpdateUserInput(
    string? NewName,
    string? NewEmail,
    string? NewPhone,
    string? NewPassword,
    string ConfirmPassword
);