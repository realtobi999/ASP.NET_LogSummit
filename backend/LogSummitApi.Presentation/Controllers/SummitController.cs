using LogSummitApi.Application.Core.Extensions;
using LogSummitApi.Domain.Core.Dto.Summits;
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
public class SummitController : ControllerBase
{
    private readonly IServiceManager _service;
    private readonly ISummitMapper _mapper;

    public SummitController(IServiceManager service, ISummitMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet("summit")]
    [HttpGet("summit/user/{userId}")]
    [HttpGet("summit/country/{country}")]
    public async Task<IActionResult> Index(Guid? userId, string? country, int limit, int offset)
    {
        var summits = (await _service.Summit.IndexAsync()).Where(r => r.IsPublic); // leave out all the private summits

        if (userId is not null)
        {
            summits = summits.Where(s => s.UserId == userId);
        }
        if (country is not null)
        {
            summits = summits.Where(s => s.Country!.Equals(country, StringComparison.CurrentCultureIgnoreCase));
        }

        return Ok(summits.Paginate(offset, limit));
    }


    [HttpGet("summit/{summitId}")]
    public async Task<IActionResult> Get(Guid summitId)
    {
        var summit = await _service.Summit.GetAsync(summitId);

        // authenticate the request
        if (!summit.IsPublic && summit.UserId != this.GetUserIdFromJwt()) throw new NotAuthorized401Exception();

        return Ok(summit);
    }

    [HttpGet("summit/valid-countries")]
    public async Task<IActionResult> GetValidCountries() => Ok(await _service.Summit.GetValidCountriesAsync());

    [HttpPost("summit")]
    public async Task<IActionResult> Create(CreateSummitDto dto)
    {
        // authenticate the request
        if (dto.UserId != this.GetUserIdFromJwt()) throw new NotAuthorized401Exception();

        var summit = _mapper.CreateEntityFromDto(dto); 

        await _service.Summit.CreateAsync(summit);
        return Created($"/v1/api/summit/{summit.Id}", null);
    }

    [HttpPut("summit/{summitId}")]
    public async Task<IActionResult> Update(Guid summitId, UpdateSummitDto dto)
    {
        var summit = await _service.Summit.GetAsync(summitId);

        // authenticate the request
        if (summit.UserId != this.GetUserIdFromJwt()) throw new NotAuthorized401Exception();

        _mapper.UpdateEntityFromDto(summit, dto);

        await _service.Summit.UpdateAsync(summit);
        return NoContent();
    }

    [HttpDelete("summit/{summitId}")]
    public async Task<IActionResult> Delete(Guid summitId)
    {
        var summit = await _service.Summit.GetAsync(summitId);

        // authenticate the request
        if (summit.UserId != this.GetUserIdFromJwt()) throw new NotAuthorized401Exception();

        await _service.Summit.DeleteAsync(summit);
        return NoContent();
    }
}
