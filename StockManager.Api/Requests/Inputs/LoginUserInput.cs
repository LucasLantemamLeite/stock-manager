namespace StockManager.Api.Requests.Inputs;

public sealed record LoginUserInput(string Email, string ConfirmPassword);