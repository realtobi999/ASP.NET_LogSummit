using LogSummitApi.Domain.Core.Dto.Summit;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.HTTP;
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

    [HttpGet("summit")]
    public async Task<IActionResult> IndexAsync(int limit, int offset)
    {
        var summits = await _service.Summit.IndexAsync();

        if (offset > 0)
        {
            summits = summits.Skip(offset);
        }
        if (limit > 0)
        {
            summits = summits.Take(limit);
        }

        return Ok(summits.Select(s => s.ToDto()).ToList()); 
    }

    [HttpGet("summit/{summitId}")]
    public async Task<IActionResult> Get(Guid summitId)
    {
        var summit = await _service.Summit.GetAsync(summitId);

        return Ok(summit.ToDto());
    }

    [HttpGet("summit/valid-countries")]
    public async Task<IActionResult> GetValidCountries()
    {
        return Ok(await _service.Summit.GetValidCountriesAsync());
    }

    [HttpPost("summit")]
    public async Task<IActionResult> Create([FromBody] CreateSummitDto createSummitDto)
    {
        var summit = await _service.Summit.CreateAsync(createSummitDto);

        return Created($"/v1/api/summit/{summit.Id}", null);
    }

    [HttpPut("summit/{summitId}")]
    public async Task<IActionResult> Update(Guid summitId, [FromBody] UpdateSummitDto updateSummitDto)
    {
        try
        {
            await _service.Summit.UpdateAsync(await _service.Summit.GetAsync(summitId), updateSummitDto);

            return NoContent();
        }
        catch (NotFound404Exception)
        {
            var summit = await _service.Summit.CreateAsync(new CreateSummitDto()
            {
                Id = summitId,
                UserId = updateSummitDto.UserId,
                Name = updateSummitDto.Name,
                Description = updateSummitDto.Description,
                Country = updateSummitDto.Country,
                Coordinate = updateSummitDto.Coordinate
            });

            return Created($"/v1/api/summit/{summit.Id}", null);
        }
    }

    [HttpDelete("summit/{summitId}")]
    public async Task<IActionResult> Delete(Guid summitId)
    {
        var summit = await _service.Summit.GetAsync(summitId);

        await _service.Summit.DeleteAsync(summit);

        return NoContent();
    }
}
