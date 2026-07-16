using Microsoft.OpenApi;

namespace StockManager.Api.Extensions.OpenApi;

public static class DocumentTransformerExtension
{
    public static void AddDocumentTransformerOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info = new OpenApiInfo()
                {
                    Title = "StockManager",
                    Version = "v1",
                    Description = "Stock Manager é um sistema de gerênciamento de empresas, usuários, produtos e armazéns"
                };

                document.Tags = new HashSet<OpenApiTag>
                {
                    new()
                    {
                        Name = "Users",
                        Description = "Rotas responsáveis pelas ações dos usuários"
                    }
                };

                return Task.CompletedTask;
            })
        );
    }
}