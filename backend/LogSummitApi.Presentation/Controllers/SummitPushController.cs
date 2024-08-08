using LogSummitApi.Domain.Core.Dto.Summit.Routes;
using LogSummitApi.Domain.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LogSummitApi.Presentation.Controllers;

[ApiController]
[Route("v1/api")]
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

        if (offset > 0)
        {
            routes = routes.Skip(offset);
        }
        if (limit > 0)
        {
            routes = routes.Take(limit);
        }

        return Ok(routes.Select(sp => sp.ToDto()).ToList());
    }

    [HttpPost("summit/route")]
    public async Task<IActionResult> Create([FromBody] CreateRouteDto createRouteDto)
    {
        var route = await _service.Route.CreateAsync(createRouteDto);

        return Created($"/v1/api/summit/route/{route.Id}", null);
    }
}
