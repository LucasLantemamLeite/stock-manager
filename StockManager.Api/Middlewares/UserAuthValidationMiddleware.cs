using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using StockManager.Api.Data.Context;

namespace StockManager.Api.Middlewares;

public sealed class UserAuthValidationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext, AppDbContext appDbContext)
    {
        var tokenIdString = httpContext.User.FindFirstValue((ClaimTypes.NameIdentifier));

        if (!Guid.TryParse(tokenIdString, out var tokenGuidId))
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await httpContext.Response.CompleteAsync();
            return;
        }

        var authenticatedUserAccount = await appDbContext.Users.SingleOrDefaultAsync(u => u.Id.Equals(tokenGuidId));

        if (authenticatedUserAccount is null)
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await httpContext.Response.CompleteAsync();
            return;
        }
        
        httpContext.Items.Add("AuthenticatedUserAccount", authenticatedUserAccount);
        
        await next(httpContext);
    }
}