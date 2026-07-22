using System.Net;
using System.Text.Json.Serialization;

namespace StockManager.Api.UseCases.Result;

public sealed record UseCaseResult<TData>(HttpStatusCode HttpStatusCode, string Message, TData? Data = null)
    where TData : class
{
    private HttpStatusCode HttpStatusCode { get; } = HttpStatusCode;
    
    [JsonIgnore]
    public int IntStatusCode => (int)HttpStatusCode;
}

public sealed record UseCaseResult(HttpStatusCode HttpStatusCode, string Message)
{
    private HttpStatusCode HttpStatusCode { get; } = HttpStatusCode;
    
    [JsonIgnore]
    public int IntStatusCode => (int)HttpStatusCode;
}