﻿using LogSummitApi.Domain.Core.Dto.Summit;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Domain.Core.Interfaces.Services;
using LogSummitApi.Domain.Core.Interfaces.Utilities;

namespace LogSummitApi.Application.Core.Services.Summits;

public class SummitService : ISummitService
{
    private readonly IRepositoryManager _repository;
    private readonly IValidator<Summit> _validator;

    public SummitService(IRepositoryManager repository, IValidator<Summit> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Summit> Create(CreateSummitDto createSummitDto)
    {

        var summit = new Summit
        {
            Id = createSummitDto.Id ?? Guid.NewGuid(),
            UserId = createSummitDto.UserId,
            Name = createSummitDto.Name,
            Description = createSummitDto.Description,
            Country = createSummitDto.Country,
            CreatedAt = DateTime.UtcNow,
            Coordinate = createSummitDto.Coordinate
        };

        // validate the object
        var (valid, exception) = await _validator.IsValidAsync(summit);
        if (!valid && exception is not null) throw exception;

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
