using Microsoft.AspNetCore.Mvc;

namespace StockManager.Api.Extensions.Configurations;

public static class ModelStateExtension
{
    public static void ConfigureModelStateFilter(this IMvcBuilder mvcBuilder)
        => mvcBuilder.ConfigureApiBehaviorOptions(options =>
            options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var allFailEntries = actionContext.ModelState
                        .Where(entry => entry.Value is not null && entry.Value.Errors.Count > 0);
            
                    var modelStateResponse = new Dictionary<string, IEnumerable<string>>();

                    foreach (var failEntry in allFailEntries)
                    {
                        if (failEntry.Value is null)
                            continue;

                        var entryErrorMessages = failEntry.Value.Errors.Select(x => x.ErrorMessage);

                        modelStateResponse[failEntry.Key] = entryErrorMessages;
                    }
            
                    return new BadRequestObjectResult(modelStateResponse);
                });
}