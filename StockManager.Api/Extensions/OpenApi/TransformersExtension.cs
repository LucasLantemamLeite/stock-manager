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
                
                    const string serverUrl = "{protocol}://localhost:5034/";
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
                    
                    document.Components ??= new OpenApiComponents();

                    document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>()
                    {
                        ["BearerAuth"] = new OpenApiSecurityScheme()
                        {
                            Type = SecuritySchemeType.Http,
                            Scheme = "bearer",
                            Description = "Informe o token de autenticação Jwt Bearer gerado pelo sistema."
                        }
                    };

                    foreach (var endpointItem in context.DescriptionGroups.SelectMany(d => d.Items))
                    {
                        var hasAuthorize = endpointItem.ActionDescriptor.EndpointMetadata
                            .OfType<IAuthorizeData>()
                            .Any();

                        var hasAllowAnonymous = endpointItem.ActionDescriptor.EndpointMetadata
                            .OfType<IAllowAnonymous>()
                            .Any();

                        if (!hasAuthorize && hasAllowAnonymous)
                            continue;

                        if (endpointItem.RelativePath is null)
                            throw new NullReferenceException("Caminho relativo para a rota não encontrado.");

                        var fullRelativePath = endpointItem.RelativePath.StartsWith('/')
                            ? endpointItem.RelativePath
                            : '/' + endpointItem.RelativePath;

                        var endpointPathItem = document.Paths[fullRelativePath];

                        if (endpointItem.HttpMethod is null)
                            throw new NullReferenceException("Método HTTP não encontrado.");

                        var endpointMethod = new HttpMethod(endpointItem.HttpMethod);

                        if (endpointPathItem.Operations is null)
                            throw new NullReferenceException("Operação da rota não encontrada.");
                        
                        var endpointOperation = endpointPathItem.Operations[endpointMethod];

                        endpointOperation.Security = new List<OpenApiSecurityRequirement>()
                        {
                            new()
                            {
                                [new OpenApiSecuritySchemeReference("BearerAuth", document)] = []
                            }
                        };
                    }
                    
                
                    return Task.CompletedTask;
                })
            );
        }

        public void AddOperationTransformerOpenApi()
        {
            services.AddOpenApi(options =>
                options.AddOperationTransformer((operation, context, cancellationToken) =>
                {
                    var defaultResponseErrorSchema = new OpenApiSchema()
                    {
                        Type = JsonSchemaType.Object,
                        Required = new HashSet<string>() { "message" },
                        Properties = new Dictionary<string, IOpenApiSchema>()
                        {
                            ["message"] = new OpenApiSchema()
                            {
                                Type = JsonSchemaType.String,
                            },
                        }
                    };

                    var defaultResponseErrorContent = new Dictionary<string, OpenApiMediaType>()
                    {
                        ["application/json"] = new()
                        {
                            Schema = defaultResponseErrorSchema
                        }
                    };

                    operation.Responses ??= new OpenApiResponses();

                    operation.Responses["500"] = new OpenApiResponse()
                    {
                        Description = "InternalServerError",
                        Content = defaultResponseErrorContent
                    };
                    
                    var hasAuthorize = context.Description.ActionDescriptor.EndpointMetadata
                        .OfType<IAuthorizeData>()
                        .Any();
                    
                    var hasAllowAnonymous = context.Description.ActionDescriptor.EndpointMetadata
                        .OfType<IAllowAnonymous>()
                        .Any();

                    if (hasAuthorize && !hasAllowAnonymous)
                    {
                        operation.Responses["401"] = new OpenApiResponse()
                        {
                            Description = "Unauthorized",
                            Content = defaultResponseErrorContent
                        };
                    }
                    
                    return Task.CompletedTask;
                })
            );
        }
    }
}