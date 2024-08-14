using System.Security.Claims;
using LogSummitApi.Domain.Core.Dto.User;
using LogSummitApi.Domain.Core.Exceptions.Http;
using LogSummitApi.Domain.Core.Interfaces.Common;
using LogSummitApi.Domain.Core.Interfaces.Services;
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
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerUserDto)
    {
        var user = await _service.User.CreateAsync(registerUserDto);

        return Created($"/v1/api/user/{user.Id}", null);
    }

    [HttpPost("auth/login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginUserDto)
    {
        // we can ignore the null warning cause of the asp.net validation  
        var user = await _service.User.GetAsync(loginUserDto.Email!);
        var authenticated = _service.User.Authenticate(user, loginUserDto.Password!);

        if (!authenticated)
        {
            throw new NotAuthorized401Exception();
        }

        var token = _jwt.Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);

        return Ok(new LoginResponseDto()
        {
            User = user.ToDto(),
            Token = token
        });
    }
}
