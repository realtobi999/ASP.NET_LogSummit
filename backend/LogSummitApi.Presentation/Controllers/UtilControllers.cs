using LogSummitApi.Application.Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogSummitApi.Presentation.Controllers;

[ApiController]
[Route("v1/api")]
public class UtilControllers : ControllerBase
{
    [HttpGet("check/health")]
    public IActionResult HealthCheck()
    {
        return Ok();
    }

    [HttpGet("check/error")]
    public IActionResult ErrorCheck()
    {
        throw new NotImplementedException();
    }

    [HttpGet("check/auth"), Authorize]
    public IActionResult AuthCheck()
    {
        return Ok();
    }
}
