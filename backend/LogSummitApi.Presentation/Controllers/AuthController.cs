using System.Security.Claims;
using LogSummitApi.Domain.Core.Dto.Users;
using LogSummitApi.Domain.Core.Interfaces.Services;
using LogSummitApi.Domain.Core.Interfaces.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace LogSummitApi.Presentation.Controllers;

[ApiController]
[Route("v1/api")]
public class AuthController : ControllerBase
{
    private readonly IServiceManager _service;
    private readonly IJwt _jwt;

    public AuthController(IServiceManager service, IJwt jwt)
    {
        _service = service;
        _jwt = jwt;
    }

    [HttpPost("auth/register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto RegisterUserDto)
    {
        var user = await _service.Users.Create(RegisterUserDto);

        var token = _jwt.Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]); 

        return Created($"/v1/api/user/{user.Id}", token);
    }
}
