using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using StockManager.Api.Extensions.Configurations;
using StockManager.Api.Extensions.OpenApi;
using StockManager.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = (actionContext) =>
        {
            var allFailEntries = actionContext.ModelState
                .Where(entry => entry.Value is not null && entry.Value.Errors.Count > 0);
            
            var modelStateResponse = new Dictionary<string, IEnumerable<string>>();

            foreach (var failEntry in allFailEntries)
            {
                if (failEntry.Value is null)
                    continue;

                var entryErrorMessages = failEntry.Value.Errors.Select(x => x.ErrorMessage);

                modelStateResponse[failEntry.Key] = entryErrorMessages;
            }
            
            return new BadRequestObjectResult(modelStateResponse);
        };
    });

var secretKey = builder.Configuration.GetValue<string>("SecretKey")
    ?? throw new InvalidOperationException("SecretKey não encontrada no appsettings.");

builder.Services.ConfigureJwtAuthentication(secretKey);

builder.ResolveDependency(secretKey);

builder.Services.AddDocumentTransformerOpenApi();
builder.Services.AddOperationTransformerOpenApi();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference("/docs",options =>
    {
        options.Title = "Stock Manager Documentation";
        options.WithOpenApiRoutePattern($"/openapi/{TransformersExtension.DocumentName}.json");
        options.ForceDarkMode();
        options.Theme = ScalarTheme.DeepSpace;
    });
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();