namespace StockManager.Api.Extensions.OpenApi;

public static class OperationTransformerExtension
{
    public static void AddOperationTransformerOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
            options.AddOperationTransformer((operation, context, cancellationToken) =>
            {


                return Task.CompletedTask;
            })
        );
    }
}