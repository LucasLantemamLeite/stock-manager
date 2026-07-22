using System.ComponentModel;
using System.Net;
using System.Text.Json.Serialization;

namespace StockManager.Api.UseCases.Result;

public sealed record UseCaseResult<TData>(HttpStatusCode HttpStatusCode, string Message, TData? Data = null)
    where TData : class
{
    private HttpStatusCode HttpStatusCode { get; } = HttpStatusCode;
    
    [Description("Mensagem informando o resultado do processo.")]
    public string Message { get; } = Message;

    [Description("Dados importantes que podem ser retornados em um processo.")]
    public TData? Data { get; } = Data;
    
    [JsonIgnore]
    public int IntStatusCode => (int)HttpStatusCode;
}

public sealed record UseCaseResult(HttpStatusCode HttpStatusCode, string Message)
{
    private HttpStatusCode HttpStatusCode { get; } = HttpStatusCode;
    
    [Description("Mensagem informando o resultado do processo.")]
    public string Message { get; } = Message;
    
    [JsonIgnore]
    public int IntStatusCode => (int)HttpStatusCode;
}