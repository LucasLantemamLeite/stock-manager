using Microsoft.OpenApi;

namespace StockManager.Api.Extensions.OpenApi;

public static class OperationTransformerExtension
{
    public static void AddOperationTransformerOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
            options.AddOperationTransformer((operation, context, cancellationToken) =>
            {
                operation?.Responses?.TryAdd("500", new OpenApiResponse()
                {
                    Description = "InternalServerError",
                    Content = new Dictionary<string, OpenApiMediaType>()
                    {
                        ["application/json"] = new OpenApiMediaType()
                        {
                            Schema = new OpenApiSchema()
                            {
                                Type = JsonSchemaType.Object,
                                Properties = new Dictionary<string, IOpenApiSchema>()
                                {
                                    ["message"] = new OpenApiSchema()
                                    {
                                        Type = JsonSchemaType.String,
                                    },
                                }
                            }
                        }
                    }
                });

                return Task.CompletedTask;
            })
        );
    }
}