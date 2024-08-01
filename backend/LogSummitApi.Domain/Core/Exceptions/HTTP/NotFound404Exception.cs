using System.Net;
using LogSummitApi.Domain.Core.Interfaces.Utilities;

namespace LogSummitApi.Domain.Core.Exceptions.HTTP;

public class NotFound404Exception(string entity, object key) : Exception($"The requested {entity} with the key '{key}' was not found in the system."), IHttpException
{
    public int StatusCode => (int)HttpStatusCode.NotFound;

    public string Title => "Resource Not Found";
}