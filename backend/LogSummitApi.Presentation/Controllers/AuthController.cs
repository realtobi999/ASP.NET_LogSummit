using System.Security.Claims;
using LogSummitApi.Domain.Core.Dto.Users;
using LogSummitApi.Domain.Core.Exceptions.Http;
using LogSummitApi.Domain.Core.Interfaces.Common;
using LogSummitApi.Domain.Core.Interfaces.Mappers;
using LogSummitApi.Domain.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LogSummitApi.Presentation.Controllers;

[ApiController]
[Route("v1/api")]
public class AuthController : ControllerBase
{
    private readonly IServiceManager _service;
    private readonly IJwt _jwt;
    private readonly IUserMapper _mapper;

    public AuthController(IServiceManager service, IJwt jwt, IUserMapper mapper)
    {
        _service = service;
        _jwt = jwt;
        _mapper = mapper;
    }

    [HttpPost("auth/register")]
    public async Task<IActionResult> RegisterUser(CreateUserDto body)
    {
        var user = _mapper.CreateEntityFromDto(body);

        await _service.User.CreateAsync(user);
        return Created($"/v1/api/user/{user.Id}", null);
    }

    [HttpPost("auth/login")]
    public async Task<IActionResult> LoginUser(LoginUserDto loginUserDto)
    {
        // we can ignore the null warnings because of the asp.net validation  
        var user = await _service.User.GetAsync(loginUserDto.Email!);

        if (!_service.User.Authenticate(user, loginUserDto.Password!))
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
