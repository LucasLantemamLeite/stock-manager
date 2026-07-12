namespace StockManager.Api.Requests.Inputs;

public sealed record DeleteUserInput(
    Guid Id,
    string ConfirmPassword
);