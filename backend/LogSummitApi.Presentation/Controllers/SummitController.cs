using LogSummitApi.Domain.Core.Dto.Summit;
using LogSummitApi.Domain.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LogSummitApi.Presentation.Controllers;

[ApiController]
[Route("v1/api")]
public class SummitController : ControllerBase
{
    private readonly IServiceManager _service;

    public SummitController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet("summit/valid-countries")]
    public async Task<IActionResult> GetSummitValidCountries()
    {
        return Ok(await _service.Summit.GetValidCountries());
    }

    [HttpPost("summit")]
    public async Task<IActionResult> CreateSummit([FromBody] CreateSummitDto createSummitDto)
    {
        var summit = await _service.Summit.Create(createSummitDto);

        return Created($"/v1/api/summit/{summit.Id}", null);
    }
}
