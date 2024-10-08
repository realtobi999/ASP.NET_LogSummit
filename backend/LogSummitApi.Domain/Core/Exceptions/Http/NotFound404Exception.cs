﻿using System.Net;
using LogSummitApi.Domain.Core.Interfaces.Common;

namespace LogSummitApi.Domain.Core.Exceptions.Http;

public class NotFound404Exception : Exception, IHttpException
{
    public NotFound404Exception(string entity, object key) : base($"The requested {entity} with the key '{key}' was not found in the system.")
    {
    }

    public int StatusCode => (int)HttpStatusCode.NotFound;
    public string Title => "Resource Not Found";
}