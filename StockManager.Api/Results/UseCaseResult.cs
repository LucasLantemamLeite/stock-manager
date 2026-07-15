using System.Net;

namespace StockManager.Api.Results;

public sealed record UseCaseResult<TData>(HttpStatusCode HttpStatusCode, string Message, TData? Data = null)
    where TData : class
{
    private HttpStatusCode HttpStatusCode { get; } = HttpStatusCode;
    public int IntStatusCode => (int)HttpStatusCode;
    public string Message { get; } = Message;
    public TData? Data { get; } = Data;
}

public sealed record UseCaseResult(HttpStatusCode HttpStatusCode, string Message)
{
    private HttpStatusCode HttpStatusCode { get; } = HttpStatusCode;
    public int IntStatusCode => (int)HttpStatusCode;
    public string Message { get; } = Message;
}