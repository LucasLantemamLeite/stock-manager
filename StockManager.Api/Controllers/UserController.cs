using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManager.Api.Contracts.Users.Inputs;
using StockManager.Api.UseCases.Users;
using System.Security.Claims;

namespace StockManager.Api.Controllers;

[ApiController]
[Route("v1")]
[Tags("Users")]
public sealed class UserController : ControllerBase
{
    [HttpPost("user")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserInput requestInput, CreateUserUseCase createUserUseCase)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var useCaseResult = await createUserUseCase.ExecuteAsync(requestInput);

        return StatusCode(useCaseResult.IntStatusCode, new { useCaseResult.Message, useCaseResult.Data });
    }

    [HttpPost("user/login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginUserAsync([FromBody] LoginUserInput requestInput, LoginUserUseCase loginUserUseCase)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var useCaseResult = await loginUserUseCase.ExecuteAsync(requestInput);

        return StatusCode(useCaseResult.IntStatusCode, new { useCaseResult.Message, useCaseResult.Data });
    }

    [HttpPatch("user")]
    [Authorize]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserInput requestInput, UpdateUserUseCase updateUserUseCase)
    {
        var tokenIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(tokenIdString, out var tokenIdGuid))
            return Unauthorized();

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var useCaseResult = await updateUserUseCase.ExecuteAsync(requestInput, tokenIdGuid);

        return StatusCode(useCaseResult.IntStatusCode, new { useCaseResult.Message });
    }

    [HttpDelete("user")]
    [Authorize]
    public async Task<IActionResult> DeleteUserAsync([FromBody] ConfirmPasswordInput requestInput, DeleteUserUseCase deleteUserUseCase)
    {
        var tokenIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(tokenIdString, out var tokenIdGuid))
            return Unauthorized();

        if (ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var useCaseResult = await deleteUserUseCase.ExecuteAsync(requestInput, tokenIdGuid);

        return StatusCode(useCaseResult.IntStatusCode, new { useCaseResult.Message });
    }
}