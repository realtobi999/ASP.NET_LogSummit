using LogSummitApi.Domain.Core.Dto.Summit;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.HTTP;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Domain.Core.Interfaces.Services;

namespace LogSummitApi.Application.Core.Services.Summits;

public class SummitService : ISummitService
{
    private readonly IRepositoryManager _repository;

    public SummitService(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<Summit> Create(CreateSummitDto createSummitDto)
    {
        // validate that the user exists
        var user = await _repository.Users.Get(createSummitDto.UserId)
            ?? throw new NotFound404Exception(nameof(User), createSummitDto.UserId);

        // check if there is already a summit within a set radius (in meters)
        var existingSummits = await _repository.Summit.Index();
        if (existingSummits.Any(s => s.Coordinate!.IsWithinRange(createSummitDto.Coordinate!, Summit.SummitProximityRadius)))
        {
            throw new BadRequest400Exception($"A summit already exists within a {Summit.SummitProximityRadius}-meter radius.");
        }

        var summit = new Summit
        {
            Id = createSummitDto.Id ?? Guid.NewGuid(),
            UserId = user.Id,
            Name = createSummitDto.Name,
            Description = createSummitDto.Description,
            CreatedAt = createSummitDto.CreatedAt,
            Coordinate = createSummitDto.Coordinate
        };

        _repository.Summit.Create(summit);
        await _repository.SaveSafelyAsync();

        return summit;
    }

    public async Task<IEnumerable<string>> GetValidCountries()
    {
        var countries = await _repository.Country.Index();

        return countries
            .Select(country =>
            {
                if (country.Name == null) throw new NullReferenceException("Country 'Name' property cannot be null.");
                if (country.Name.Common == null) throw new NullReferenceException("Country 'Name.Common' property cannot be null.");

                return country.Name.Common;
            })
            .ToList();
    }
}
