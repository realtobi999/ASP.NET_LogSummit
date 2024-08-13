using System.Net;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Interfaces.Common;
using Microsoft.AspNetCore.Diagnostics;

namespace LogSummitApi.Presentation.Middleware.Handlers;

public class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken token)
    {
        var error = new ErrorMessage()
        {
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

