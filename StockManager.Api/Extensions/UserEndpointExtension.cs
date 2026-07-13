using Microsoft.AspNetCore.Mvc;
using StockManager.Api.Requests.Inputs;
using StockManager.Api.UseCases;

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

                if (useCaseResult.StopExecution)
                    return Results.Json(new { useCaseResult.Message, }, statusCode: useCaseResult.IntStatusCode);

                return Results.Json(new
                {
                    useCaseResult.Message,
                    token = useCaseResult.Data
                }, statusCode: useCaseResult.IntStatusCode);
            })
                .AllowAnonymous();

            app.MapPost("/v1/user/login", async ([FromBody] LoginUserInput requestInput, LoginUserUseCase loginUserUseCase) =>
            {
                var useCaseResult = await loginUserUseCase.ExecuteAsync(requestInput);

                if (useCaseResult.StopExecution)
                    return Results.Json(new { useCaseResult.Message }, statusCode: useCaseResult.IntStatusCode);

                return Results.Json(new
                {
                    useCaseResult.Message,
                    token = useCaseResult.Data
                }, statusCode: useCaseResult.IntStatusCode);
            })
                .AllowAnonymous();

            app.MapPatch("/v1/user", async ([FromBody] UpdateUserInput requestInput, UpdateUserUseCase updateUserUseCase) =>
            {
                var useCaseResult = await updateUserUseCase.ExecuteAsync(requestInput);

                if (useCaseResult.StopExecution)
                    return Results.Json(new { useCaseResult.Message }, statusCode: useCaseResult.IntStatusCode);

                return Results.Json(new { useCaseResult.Message }, statusCode: useCaseResult.IntStatusCode);
            });

            app.MapDelete("/v1/user", async ([FromBody] DeleteUserInput requestInput, DeleteUserUseCase deleteUserUseCase) =>
            {
                var useCaseResult = await deleteUserUseCase.ExecuteAsync(requestInput);

                if (useCaseResult.StopExecution)
                    return Results.Json(new { useCaseResult.Message }, statusCode: useCaseResult.IntStatusCode);

                return Results.Json(new { useCaseResult.Message }, statusCode: useCaseResult.IntStatusCode);
            });
        }
    }
}