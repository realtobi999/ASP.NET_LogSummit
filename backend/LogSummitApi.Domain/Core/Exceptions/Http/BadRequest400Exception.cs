using System.Net;
using LogSummitApi.Domain.Core.Interfaces.Common;

namespace LogSummitApi.Domain.Core.Exceptions.Http;

public class BadRequest400Exception : Exception, IHttpException
{
    public BadRequest400Exception(string message) : base(message)
    {
    }

    public int StatusCode => (int)HttpStatusCode.BadRequest;
    public string Title => "Bad Request";
}