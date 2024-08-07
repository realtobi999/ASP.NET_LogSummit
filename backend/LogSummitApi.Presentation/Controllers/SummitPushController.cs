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

    [HttpPost("summit/push")]
    public async Task<IActionResult> Create([FromBody] CreateSummitPushDto createSummitPushDto)
    {
        var summitPush = await _service.SummitPush.CreateAsync(createSummitPushDto);

        return Created($"/v1/api/summit/push/{summitPush.Id}", null);
    }
}
