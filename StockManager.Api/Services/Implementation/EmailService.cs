using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using StockManager.Api.Services.Interfaces;

namespace StockManager.Api.Services.Implementation;

public sealed class EmailService(string resendApiKey) : IEmailService
{
    private readonly Uri _resendSendEmailUrl =new Uri("https://api.resend.com/");
    
    public async Task Send(string subject, string htmlContent)
    {
        var httpClient = new HttpClient()
        {
            BaseAddress = _resendSendEmailUrl
        };

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", resendApiKey);

        var bodyRequest = new
        { 
            from = "onboarding@resend.dev",
            to = "lucaslantemamleite2005@gmail.com",
            subject,
            html = htmlContent
        };

        var bodyRequestSerialization = JsonSerializer.Serialize(bodyRequest);

        var requestContent = new StringContent(bodyRequestSerialization, Encoding.UTF8, "application/json");

        await httpClient.PostAsync("emails", requestContent);
    }
}