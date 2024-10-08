﻿using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.Http;
using LogSummitApi.Domain.Core.Interfaces.Common;
using LogSummitApi.Domain.Core.Interfaces.Repositories;

namespace LogSummitApi.Application.Core.Validators;

public class SummitValidator : IValidator<Summit>
{
    private readonly IRepositoryManager _repository;

    public SummitValidator(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<(bool isValid, Exception? exception)> IsValidAsync(Summit summit)
    {
        var summits = await _repository.Summit.IndexAsync();
        var countries = await _repository.HttpCountry.IndexAsync();

        // validate that the user exists
        if (await _repository.Users.GetAsync(summit.UserId) is null)
        {
            return (false, new NotFound404Exception(nameof(User), summit.UserId));
        }

        // check if there is already a public summit within a set radius (only if the validation summit is set to public)
        if (summits.Any(existingSummit =>
        {
            return existingSummit.Id != summit.Id && 
            existingSummit.IsPublic && 
            summit.IsPublic && 
            existingSummit.Coordinate!.IsWithinDistanceTo(summit.Coordinate!, Summit.SUMMIT_PROXIMITY_RADIUS);
        }))
        {
            return (false, new BadRequest400Exception($"A summit already exists within a {Summit.SUMMIT_PROXIMITY_RADIUS}-meter radius."));
        }

        // ensure that the country is valid
        if (!countries.Any(c => c.Name!.Common == summit.Country))
        {
            return (false, new BadRequest400Exception($"A '{summit.Country}' is not a valid country. List of all available countries: GET /v1/api/summit/valid-countries"));
        }

        return (true, null);
    }
}
