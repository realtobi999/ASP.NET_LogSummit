using LogSummitApi.Application.Core.Extensions;
using LogSummitApi.Domain.Core.Dto.Summit.Routes;
using LogSummitApi.Domain.Core.Exceptions.Http;
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

    public RouteController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet("summit/route")]
    public async Task<IActionResult> Index(int limit, int offset)
    {
        var routes = await _service.Route.IndexAsync();

        routes = routes.Where(r => r.IsPublic);

        return Ok(routes.Paginate(offset, limit));
    }

    [HttpGet("summit/route/{routeId}")]
    public async Task<IActionResult> Get(Guid routeId)
    {
        var route = await _service.Route.GetAsync(routeId);

        if (!route.IsPublic && route.UserId != this.GetUserIdFromJwt()) throw new NotAuthorized401Exception();

        return Ok(route);
    }

    [HttpPost("summit/route")]
    public async Task<IActionResult> Create([FromBody] CreateRouteDto createRouteDto)
    {
        // authenticate the request
        if (createRouteDto.UserId != this.GetUserIdFromJwt()) throw new NotAuthorized401Exception();

        var route = await _service.Route.CreateAsync(createRouteDto);

        return Created($"/v1/api/summit/route/{route.Id}", null);
    }

    [HttpPut("summit/route/{routeId}")]
    public async Task<IActionResult> Update(Guid routeId, [FromBody] UpdateRouteDto updateRouteDto)
    {
        var route = await _service.Route.GetAsync(routeId);

        // authenticate the request
        if (route.UserId != this.GetUserIdFromJwt()) throw new NotAuthorized401Exception();

        await _service.Route.UpdateAsync(route, updateRouteDto);
        return NoContent();
    }

    [HttpDelete("summit/route/{routeId}")]
    public async Task<IActionResult> Delete(Guid routeId)
    {
        var route = await _service.Route.GetAsync(routeId);

        // authenticate the request
        if (route.UserId != this.GetUserIdFromJwt()) throw new NotAuthorized401Exception();

        await _service.Route.DeleteAsync(route);
        return NoContent();
    }
}
