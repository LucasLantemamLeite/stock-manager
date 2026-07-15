namespace StockManager.Api.Middlewares;

public sealed class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }

        catch (Exception ex)
        {
            logger.LogWarning(ex, "Uma exceção foi capturada.");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var errorMessage = new { message = "Ocorreu um erro interno no servidor. Tente novamente mais tarde." };

            await context.Response.WriteAsJsonAsync(errorMessage);
        }
    }
}