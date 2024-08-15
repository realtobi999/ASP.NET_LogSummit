using LogSummitApi.Domain.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LogSummitApi.Presentation.Middleware.Filters;

/// <summary>
/// A result filter that wraps the result of a controller action in a <c>SuccessMessage</c> object.
/// <para>
/// This filter is intended to standardize the response format for successful results by encapsulating the 
/// data in a <c>SuccessMessage</c> which includes a success flag, the data itself, and the request information.
/// </para>
/// <para>
/// The <c>SuccessMessage</c> object will be populated with:
/// <list type="bullet">
///     <item><description><c>Success</c> set to <c>true</c></description></item>
///     <item><description><c>Data</c> containing the original result data from the action, or an empty object if the result is null.</description></item>
///     <item><description><c>Instance</c> containing the HTTP method and request path of the request.</description></item>
/// </list>
/// </para>
/// </summary>
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
