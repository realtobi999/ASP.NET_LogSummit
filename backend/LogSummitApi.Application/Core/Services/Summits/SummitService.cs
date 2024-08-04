﻿using LogSummitApi.Domain.Core.Dto.Summit;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.HTTP;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Domain.Core.Interfaces.Services;

namespace LogSummitApi.Application.Core.Services.Summits;

public class SummitService : ISummitService
{
    private readonly IRepositoryManager _repository;
    private static List<CountryDto>? _countries; // make the property static so we don't have to call the API multiple times

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

        // ensure that the country is valid
        if (!(await this.GetValidCountries()).Contains(createSummitDto.Country))
        {
            throw new BadRequest400Exception($"A {createSummitDto.Country}' is not a valid country. List of all available countries: GET /v1/api/summit/valid-countries");
        }

        var summit = new Summit
        {
            Id = createSummitDto.Id ?? Guid.NewGuid(),
            UserId = user.Id,
            Name = createSummitDto.Name,
            Description = createSummitDto.Description,
            Country = createSummitDto.Country,
            CreatedAt = DateTime.UtcNow,
            Coordinate = createSummitDto.Coordinate
        };

        _repository.Summit.Create(summit);
        await _repository.SaveSafelyAsync();

        return summit;
    }

    public async Task<IEnumerable<string>> GetValidCountries()
    {
        if (_countries == null)
        {
            _countries = (await _repository.Country.Index()).ToList();
        }

        return _countries
            .Select(country =>
            {
                if (country.Name == null) throw new NullReferenceException("Country 'Name' property cannot be null.");
                if (country.Name.Common == null) throw new NullReferenceException("Country 'Name.Common' property cannot be null.");

                return country.Name.Common;
            })
            .ToList();
    }
}
