namespace StockManager.Api.Services.Interfaces;

public interface IEmailService
{
    public Task Send(string subject, string htmlContent);
}