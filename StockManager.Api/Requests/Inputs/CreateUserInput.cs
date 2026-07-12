namespace StockManager.Api.Requests.Inputs;

public sealed record CreateUserInput(
    string Name,
    string Email,
    string Phone,
    sbyte Role
);