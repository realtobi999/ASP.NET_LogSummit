using System.Net;
using LogSummitApi.Domain.Core.Interfaces.Utilities;

namespace LogSummitApi.Domain.Core.Exceptions.Http;

public class ServiceUnavailable503Exception : Exception, IHttpException
{
    public ServiceUnavailable503Exception(string message) : base(message)
    {
    }

    public int StatusCode => (int)HttpStatusCode.ServiceUnavailable;
    public string Title => "Service Unavailable";
}
