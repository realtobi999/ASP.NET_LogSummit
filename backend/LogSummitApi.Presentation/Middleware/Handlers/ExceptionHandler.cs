using System.Net;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Interfaces.Common;
using Microsoft.AspNetCore.Diagnostics;

namespace LogSummitApi.Presentation.Middleware.Handlers;

/// <summary>
/// An exception handler that processes exceptions and formats error responses for HTTP requests.
/// <para>
/// This handler is responsible for generating a standardized error response based on the type of exception encountered.
/// It distinguishes between HTTP-specific exceptions and general exceptions to provide appropriate error details.
/// </para>
/// <para>
/// The error response includes:
/// <list type="bullet">
///     <item><description><c>StatusCode</c> - HTTP status code representing the error.</description></item>
///     <item><description><c>Type</c> - The type of the exception that occurred.</description></item>
///     <item><description><c>Title</c> - A brief, human-readable title of the error (used for HTTP-specific exceptions).</description></item>
///     <item><description><c>Detail</c> - A detailed message describing the error.</description></item>
///     <item><description><c>Instance</c> - The HTTP method and path of the request where the error occurred.</description></item>
/// </list>
/// </para>
/// <para>
/// Specifically:
/// <list type="bullet">
///     <item><description>If the exception is of type <c>IHttpException</c>, a detailed HTTP-specific error response is generated.</description></item>
///     <item><description>For all other exceptions, a generic internal server error response is provided.</description></item>
/// </list>
/// </para>
/// </summary>
public class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken token)
    {
        var error = new ErrorMessage()
        {
            Success = false,
            Type = exception.GetType().Name,
            Instance = $"{context.Request.Method} {context.Request.Path}"
        };

        if (exception is IHttpException httpException)
        {
            await HandleHttpException(context, httpException, token);
        }
        else
        {
            await HandleGeneralException(context, exception, token);
        }

        return false;
    }

    private static async Task HandleHttpException(HttpContext context, IHttpException exception, CancellationToken token)
    {
        var error = new ErrorMessage
        {
            StatusCode = exception.StatusCode,
            Type = exception.GetType().Name,
            Title = exception.Title,
            Detail = exception.Message,
            Instance = $"{context.Request.Method} {context.Request.Path}"
        };

        await WriteErrorAsync(context, error, token);
    }

    private static async Task HandleGeneralException(HttpContext context, Exception exception, CancellationToken token)
    {
        var error = new ErrorMessage
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Type = exception.GetType().Name,
            Title = "An unexpected internal error occurred",
            Detail = exception.Message,
            Instance = $"{context.Request.Method} {context.Request.Path}"
        };

        await WriteErrorAsync(context, error, token);
    }

    private static async Task WriteErrorAsync(HttpContext context, ErrorMessage error, CancellationToken token)
    {
        context.Response.StatusCode = error.StatusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(error, token);
    }
}

