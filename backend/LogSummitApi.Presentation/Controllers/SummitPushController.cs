using LogSummitApi.Domain.Core.Dto.Summit.Pushes;
using LogSummitApi.Domain.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LogSummitApi.Presentation.Controllers;

[ApiController]
[Route("v1/api")]
public class SummitPushController : ControllerBase
{
    private readonly IServiceManager _service;

    public SummitPushController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet("summit/push")]
    public async Task<IActionResult> Index(int limit, int offset)
    {
        var summitPushes = await _service.SummitPush.IndexAsync();

        if (offset > 0)
        {
            summitPushes = summitPushes.Skip(offset);
        }
        if (limit > 0)
        {
            summitPushes = summitPushes.Take(limit);
        }

        return Ok(summitPushes.Select(sp => sp.ToDto()).ToList());
    }

    [HttpPost("summit/push")]
    public async Task<IActionResult> Create([FromBody] CreateSummitPushDto createSummitPushDto)
    {
        var summitPush = await _service.SummitPush.CreateAsync(createSummitPushDto);

        return Created($"/v1/api/summit/push/{summitPush.Id}", null);
    }
}
