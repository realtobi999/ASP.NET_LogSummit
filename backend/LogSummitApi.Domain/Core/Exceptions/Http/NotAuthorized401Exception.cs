using System.Net;
using LogSummitApi.Domain.Core.Interfaces.Utilities;

namespace LogSummitApi.Domain.Core.Exceptions.Http;

public class NotAuthorized401Exception : Exception, IHttpException
{
    public NotAuthorized401Exception()
        : base($"You are not authorized to perform the action. Please ensure you have the necessary permissions.")
    {
    }

    public NotAuthorized401Exception(string action)
        : base($"You are not authorized to perform the action: {action}. Please ensure you have the necessary permissions.")
    {
    }

    public int StatusCode => (int)HttpStatusCode.Unauthorized;
    public string Title => "Unauthorized Access";
}