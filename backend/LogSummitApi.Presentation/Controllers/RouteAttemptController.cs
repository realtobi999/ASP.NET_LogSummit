using LogSummitApi.Application.Core.Extensions;
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

    [HttpGet("route/attempt")]
    public async Task<IActionResult> Index(Guid? routeId, Guid? userId, Guid? summitId, int limit, int offset)
    {
        var attempts = (await _service.RouteAttempt.IndexAsync()).Where(a => a.IsPublic);

        if (userId is not null)
        {
            attempts = attempts.Where(r => r.UserId == userId);
        }
        if (summitId is not null)
        {
            attempts = attempts.Where(r => r.SummitId == summitId);
        }
        if (routeId is not null)
        {
            attempts = attempts.Where(r => r.RouteId == routeId);
        }

        return Ok(attempts.Paginate(offset, limit));
    }

    [HttpGet("route/attempt/{attemptId:guid}")]
    public async Task<IActionResult> Get(Guid attemptId)
    {
        var attempt = await _service.RouteAttempt.GetAsync(attemptId);

        if (!attempt.IsPublic && attempt.UserId != this.GetUserIdFromJwt()) throw new NotAuthorized401Exception();

        return Ok(attempt);
    }

    [HttpPost("route/attempt")]
    public async Task<IActionResult> Create(CreateRouteAttemptDto dto)
    {
        // authenticate the request
        if (dto.UserId != this.GetUserIdFromJwt()) throw new NotAuthorized401Exception();
        
        var attempt = _mapper.CreateEntityFromDto(dto);

        await _service.RouteAttempt.CreateAsync(attempt);
        return Created($"/v1/api/route/attempt/{attempt.Id}", null);
    }

    [HttpPut("route/attempt/{attemptId:guid}")]
    public async Task<IActionResult> Update(Guid attemptId, UpdateRouteAttemptDto dto)
    {
        var attempt = await _service.RouteAttempt.GetAsync(attemptId);

        // authenticate the request
        if (attempt.UserId != this.GetUserIdFromJwt()) throw new NotAuthorized401Exception();

        _mapper.UpdateEntityFromDto(attempt, dto);

        await _service.RouteAttempt.UpdateAsync(attempt);
        return NoContent();
    }

    [HttpDelete("route/attempt/{attemptId:guid}")]
    public async Task<IActionResult> Delete(Guid attemptId)
    {
        var attempt = await _service.RouteAttempt.GetAsync(attemptId);

        // authenticate the request
        if (attempt.UserId != this.GetUserIdFromJwt()) throw new NotAuthorized401Exception();

        await _service.RouteAttempt.DeleteAsync(attempt);
        return NoContent();
    }
}
