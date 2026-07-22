using Scalar.AspNetCore;
using StockManager.Api.Extensions.Configurations;
using StockManager.Api.Extensions.OpenApi;
using StockManager.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

var secretKey = builder.Configuration.GetValue<string>("SecretKey")
    ?? throw new InvalidOperationException("SecretKey não encontrada no appsettings.");

builder.Services.ConfigureJwtAuthentication(secretKey);

builder.ResolveDependency(secretKey);

builder.Services.AddDocumentTransformerOpenApi();
builder.Services.AddOperationTransformerOpenApi();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.MapOpenApi();

app.MapScalarApiReference();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();