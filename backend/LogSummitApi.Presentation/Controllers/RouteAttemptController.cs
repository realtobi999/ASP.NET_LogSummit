using LogSummitApi.Domain.Core.Dto.Summits.Routes.Attempts;
using LogSummitApi.Domain.Core.Exceptions.Http;
using LogSummitApi.Domain.Core.Interfaces.Mappers;
using LogSummitApi.Domain.Core.Interfaces.Services;
using LogSummitApi.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogSummitApi.Presentation.Controllers;

[ApiController]
[Route("v1/api")]
[Authorize(Policy = "User")]
public class RouteAttemptController : ControllerBase
{
    private readonly IServiceManager _service;
    private readonly IRouteAttemptMapper _mapper;

    public RouteAttemptController(IServiceManager service, IRouteAttemptMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpPatch("route/attempt")]
    public async Task<IActionResult> Create(CreateRouteAttemptDto dto)
    {
        // authenticate the request
        if (dto.UserId != this.GetUserIdFromJwt()) throw new NotAuthorized401Exception();
        
        var attempt = _mapper.CreateEntityFromDto(dto);

        await _service.RouteAttempt.CreateAsync(attempt);
        return Created($"/v1/api/route/attempt/{attempt.Id}", null);
    }
}
