using StockManager.Api.Entities.Users.Models;
using StockManager.Api.Middlewares;

namespace StockManager.Api.Extensions.Helpers;

public static class HttpContextExtension
{
    public static User GetAuthenticatedUserFromItems(this HttpContext httpContext)
        =>  httpContext.Items[UserAuthValidationMiddleware.HttpContextItemsKey] as User 
            ?? throw new InvalidOperationException($"{UserAuthValidationMiddleware.HttpContextItemsKey} não encontrado no escopo da requisição.");
}