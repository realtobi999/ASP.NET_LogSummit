using LogSummitApi.Application.Core.Extensions;
using LogSummitApi.Domain.Core.Dto.Summits.Routes;
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
public class RouteController : ControllerBase
{
    private readonly IServiceManager _service;
    private readonly IRouteMapper _mapper;

    public RouteController(IServiceManager service, IRouteMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet("route")]
    [HttpGet("route/user/{userId}")]
    [HttpGet("route/summit/{summitId}")]
    public async Task<IActionResult> Index(Guid? userId, Guid? summitId, int limit, int offset)
    {
        var routes = await _service.Route.IndexAsync();

        routes = routes.Where(r => r.IsPublic);

        if (userId is not null)
        {
            routes = routes.Where(r => r.UserId == userId);
        }
        if (summitId is not null)
        {
            routes = routes.Where(r => r.SummitId == summitId);
        }

        return Ok(routes.Paginate(offset, limit));
    }

    [HttpGet("route/{routeId}")]
    public async Task<IActionResult> Get(Guid routeId)
    {
        var route = await _service.Route.GetAsync(routeId);

        if (!route.IsPublic && route.UserId != this.GetUserIdFromJwt()) throw new NotAuthorized401Exception();

        return Ok(route);
    }

    [HttpPost("route")]
    public async Task<IActionResult> Create(CreateRouteDto dto)
    {
        // authenticate the request
        if (dto.UserId != this.GetUserIdFromJwt()) throw new NotAuthorized401Exception();

        var route = _mapper.CreateEntityFromDto(dto); 

        await _service.Route.CreateAsync(route);
        return Created($"/v1/api/route/{route.Id}", null);
    }

    [HttpPut("route/{routeId}")]
    public async Task<IActionResult> Update(Guid routeId, UpdateRouteDto dto)
    {
        var route = await _service.Route.GetAsync(routeId);

        // authenticate the request
        if (route.UserId != this.GetUserIdFromJwt()) throw new NotAuthorized401Exception();

        _mapper.UpdateEntityFromDto(route, dto);

        await _service.Route.UpdateAsync(route);
        return NoContent();
    }

    [HttpDelete("route/{routeId}")]
    public async Task<IActionResult> Delete(Guid routeId)
    {
        var route = await _service.Route.GetAsync(routeId);

        // authenticate the request
        if (route.UserId != this.GetUserIdFromJwt()) throw new NotAuthorized401Exception();

        await _service.Route.DeleteAsync(route);
        return NoContent();
    }
}
