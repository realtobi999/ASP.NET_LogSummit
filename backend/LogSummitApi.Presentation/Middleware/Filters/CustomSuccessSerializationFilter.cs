using LogSummitApi.Domain.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LogSummitApi.Presentation.Middleware.Filters;

public class CustomSuccessSerializationFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult objectResult)
        {
            objectResult.Value = new SuccessMessage()
            {
                Success = true,
                Data = objectResult.Value ?? new(),
                Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}"
            };
        }

        await next();
    }
}
