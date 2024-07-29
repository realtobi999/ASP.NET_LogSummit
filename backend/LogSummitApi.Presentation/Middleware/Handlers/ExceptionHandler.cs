using System.Net;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.HTTP;
using LogSummitApi.Domain.Core.Interfaces.Utilities;
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

        if (exception is IHttpException httpException) // this is a planned http exception
        {
            error.StatusCode = httpException.StatusCode;
            error.Title = httpException.Title;
            error.Detail = httpException.Message;

            await WriteError(context, error, token);
        }
        else // this is not an planned http exception => default 500
        {
            error.StatusCode = (int)HttpStatusCode.InternalServerError;
            error.Title = "An unexpected internal error occurred";
            error.Detail = exception.Message;

            await WriteError(context, error, token);
        }

        return false;
    }

    private static async Task WriteError(HttpContext context, ErrorMessage error, CancellationToken token)
    {
        context.Response.StatusCode = error.StatusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync<ErrorMessage>(error, token);
    }
}
