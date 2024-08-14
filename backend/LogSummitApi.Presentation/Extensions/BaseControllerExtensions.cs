using LogSummitApi.Application.Core.Utilities;
using LogSummitApi.Domain.Core.Exceptions.Http;
using Microsoft.AspNetCore.Mvc;

namespace LogSummitApi.Presentation.Extensions;

public static class ControllerBaseExtensions
{
    public static Guid GetUserIdFromJwt(this ControllerBase controller)
    {
        var jwt = JwtUtils.ParseFromHeader(controller.HttpContext.Request.Headers.Authorization);
        var userId = JwtUtils.ParseFromPayload(jwt, "UserId") ?? throw new NotAuthorized401Exception();

        return Guid.Parse(userId);
    }
}
