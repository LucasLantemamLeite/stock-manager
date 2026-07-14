using Microsoft.AspNetCore.Mvc;
using StockManager.Api.Requests.Inputs;
using StockManager.Api.UseCases;
using System.Security.Claims;

namespace StockManager.Api.Extensions;

public static class UserEndpointExtension
{
    extension(WebApplication app)
    {
        public void AddUserEndpoints()
        {
            app.MapPost("/v1/user", async ([FromBody] CreateUserInput requestInput, CreateUserUseCase createUserUseCase) =>
            {
                var useCaseResult = await createUserUseCase.ExecuteAsync(requestInput);

                return Results.Json(new
                {
                    useCaseResult.Message,
                    useCaseResult.Data
                }, statusCode: useCaseResult.IntStatusCode);
            }).AllowAnonymous();

            app.MapPost("/v1/user/login", async ([FromBody] LoginUserInput requestInput, LoginUserUseCase loginUserUseCase) =>
            {
                var useCaseResult = await loginUserUseCase.ExecuteAsync(requestInput);

                return Results.Json(new
                {
                    useCaseResult.Message,
                    useCaseResult.Data
                }, statusCode: useCaseResult.IntStatusCode);
            }).AllowAnonymous();

            app.MapPatch("/v1/user", async ([FromBody] UpdateUserInput requestInput, UpdateUserUseCase updateUserUseCase, ClaimsPrincipal token) =>
            {
                var tokenIdString = token.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!Guid.TryParse(tokenIdString, out var tokenIdGuid))
                    return Results.Unauthorized();

                var useCaseResult = await updateUserUseCase.ExecuteAsync(requestInput, tokenIdGuid);

                return Results.Json(new
                {
                    useCaseResult.Message,
                    useCaseResult.Data
                }, statusCode: useCaseResult.IntStatusCode);
            }).RequireAuthorization();

            app.MapDelete("/v1/user", async ([FromBody] DeleteUserInput requestInput, DeleteUserUseCase deleteUserUseCase, ClaimsPrincipal token) =>
            {
                var tokenIdString = token.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!Guid.TryParse(tokenIdString, out var tokenIdGuid))
                    return Results.Unauthorized();

                var useCaseResult = await deleteUserUseCase.ExecuteAsync(requestInput, tokenIdGuid);

                return Results.Json(new
                {
                    useCaseResult.Message,
                    useCaseResult.Data
                }, statusCode: useCaseResult.IntStatusCode);
            }).RequireAuthorization();
        }
    }
}