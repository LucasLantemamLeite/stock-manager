using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManager.Api.Contracts.Companies.Inputs;
using StockManager.Api.UseCases.Companies;

namespace StockManager.Api.Controllers;

[ApiController]
[Tags("Companies")]
[Route("v1/company")]
public sealed class CompanyController : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateCompanyAsync([FromServices] CreateCompanyUseCase createCompanyUseCase, 
        [FromBody] CreateCompanyInput createCompanyInput)
    {
        var useCaseResult = await createCompanyUseCase.ExecuteAsync(createCompanyInput);

        return StatusCode(useCaseResult.IntStatusCode, useCaseResult);
    }
}