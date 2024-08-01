using System.Net;
using LogSummitApi.Domain.Core.Interfaces.Utilities;

namespace LogSummitApi.Domain.Core.Exceptions.HTTP;

public class BadRequestException(string message) : Exception(message), IHttpException
{
    public int StatusCode => (int)HttpStatusCode.BadRequest;
    public string Title => "Bad request";
}