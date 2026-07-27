using Microsoft.AspNetCore.Authorization;
using Scalar.AspNetCore;
using StockManager.Api.Extensions.Configurations;
using StockManager.Api.Extensions.OpenApi;
using StockManager.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureModelStateFilter();

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

    app.MapScalarApiReference("/docs", options =>
    {
        options.Title = "Stock Manager Documentation";
        options.WithOpenApiRoutePattern($"/openapi/{TransformersExtension.DocumentName}.json");
        options.ForceDarkMode();
        options.Theme = ScalarTheme.DeepSpace;
    });
}

app.UseAuthentication();

app.UseWhen(httpContext => httpContext.GetEndpoint()?.Metadata.OfType<IAuthorizeData>().Any() ?? false,
    branch => branch.UseMiddleware<UserAuthValidationMiddleware>());

app.UseAuthorization();

app.MapControllers();

app.Run();