using Microsoft.OpenApi;

namespace StockManager.Api.Extensions.OpenApi;

public static class TransformersExtension
{
    extension(IServiceCollection services)
    {
        public void AddDocumentTransformerOpenApi()
        {
            services.AddOpenApi(options =>
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    const string infoTitle = "Stock Manager | v1";
                    const string infoDescription = "Stock Manager é um sistema de gerênciamento de empresas, usuários, produtos e armazéns.";
                    const string infoVersion = "1.0.0";
              
                    document.Info = new OpenApiInfo()
                    {
                        Title = infoTitle,
                        Description = infoDescription,
                        Version = infoVersion,
                    };
                
                    const string serverUrl = "{protocol}://localhost:5034/v1";
                    const string serverDescription = "Rota base rodando na máquina local.";
                    var serverVariables = new Dictionary<string, OpenApiServerVariable>()
                    {
                        ["protocol"] = new()
                        {
                            Enum = ["http", "https"],
                            Default = "https"
                        }
                    };
                
                    document.Servers = new List<OpenApiServer>()
                    {
                        new()
                        {
                            Url = serverUrl,
                            Description = serverDescription,
                            Variables = serverVariables
                        },
                    };
            
                    document.Tags = new HashSet<OpenApiTag>()
                    {
                        new()
                        {
                            Name = "Users",
                            Description = "Responsável pelas operações referente ao usuário."
                        }
                    };
                
                    return Task.CompletedTask;
                })
            );
        }

        public void AddOperationTransformerOpenApi()
        {
            services.AddOpenApi(options =>
                options.AddOperationTransformer((operation, context, cancellationToken) =>
                {
                    var responseInternalServerErrorSchema = new OpenApiSchema()
                    {
                        Type = JsonSchemaType.Object,
                        Properties = new Dictionary<string, IOpenApiSchema>()
                        {
                            ["message"] = new OpenApiSchema()
                            {
                                Type = JsonSchemaType.String,
                            }
                        }
                    };

                    var responseInternalServerErrorContent = new Dictionary<string, OpenApiMediaType>()
                    {
                        ["application/json"] = new()
                        {
                            Schema = responseInternalServerErrorSchema
                        }
                    };

                    operation.Responses ??= new OpenApiResponses();

                    operation.Responses["500"] = new OpenApiResponse()
                    {
                        Description = "InternalServerError",
                        Content = responseInternalServerErrorContent
                    };
                    
                    return Task.CompletedTask;
                })
            );
        }
    }
}