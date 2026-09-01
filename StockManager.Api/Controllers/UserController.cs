using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManager.Api.Contracts.Users.Inputs;
using StockManager.Api.Extensions.Helpers;
using StockManager.Api.UseCases.Result;
using StockManager.Api.UseCases.Users;

namespace StockManager.Api.Controllers;

[ApiController]
[Route("v1/user")]
[Tags("Users")]
public sealed class UserController : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "2,3")]
    [EndpointDescription("Cria nova conta do usuário.")]
    [ProducesResponseType<UseCaseResult<string>>(StatusCodes.Status201Created)]
    [ProducesResponseType<UseCaseResult<string>>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserInput requestInput,
        CreateUserUseCase createUserUseCase)
    {
        var authenticatedUserAccount = HttpContext.GetAuthenticatedUserFromItems();

        var useCaseResult = await createUserUseCase.ExecuteAsync(requestInput, authenticatedUserAccount);

        return StatusCode(useCaseResult.IntStatusCode, useCaseResult);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [EndpointDescription("Loga na conta do usuário.")]
    [ProducesResponseType<UseCaseResult<string>>(StatusCodes.Status200OK)]
    [ProducesResponseType<UseCaseResult<string>>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginUserAsync([FromBody] LoginUserInput requestInput,
        LoginUserUseCase loginUserUseCase)
    {
        var useCaseResult = await loginUserUseCase.ExecuteAsync(requestInput);

        return StatusCode(useCaseResult.IntStatusCode, useCaseResult);
    }

    [HttpPatch]
    [Authorize]
    [EndpointDescription("Atualiza dados da conta do usuário.")]
    [ProducesResponseType<UseCaseResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<UseCaseResult>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<UseCaseResult>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserInput requestInput,
        UpdateUserUseCase updateUserUseCase)
    {
        var authenticatedUserAccount = HttpContext.GetAuthenticatedUserFromItems();

        var useCaseResult = await updateUserUseCase.ExecuteAsync(requestInput, authenticatedUserAccount);

        return StatusCode(useCaseResult.IntStatusCode, useCaseResult);
    }

    [HttpDelete]
    [Authorize]
    [EndpointDescription("Deleta a conta do usuário.")]
    [ProducesResponseType<UseCaseResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<UseCaseResult>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteUserAsync([FromBody] ConfirmPasswordInput requestInput,
        DeleteUserUseCase deleteUserUseCase)
    {
        var authenticatedUserAccount = HttpContext.GetAuthenticatedUserFromItems();

        var useCaseResult = await deleteUserUseCase.ExecuteAsync(requestInput, authenticatedUserAccount);

        return StatusCode(useCaseResult.IntStatusCode, useCaseResult);
    }

    [HttpPatch("password")]
    [AllowAnonymous]
    [EndpointDescription("Atualiza a senha do usuário temporária para uma nova senha da escolha do próprio.")]
    [ProducesResponseType<UseCaseResult<string>>(StatusCodes.Status200OK)]
    [ProducesResponseType<UseCaseResult<string>>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateUserPasswordAysnc([FromBody] ChangeTemporaryPasswordInput changeTemporaryPasswordInput, 
        ChangeTemporaryPasswordUserUseCase changeTemporaryPasswordUserUseCase)
    {
        var useCaseResult = await changeTemporaryPasswordUserUseCase.ExecuteAsync(changeTemporaryPasswordInput);

        return StatusCode(useCaseResult.IntStatusCode, useCaseResult);
    }
}