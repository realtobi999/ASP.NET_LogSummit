using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.HTTP;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Domain.Core.Interfaces.Utilities;

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
        // validate that the user exists
        if (_repository.Users.Get(summit.UserId) is null)
        {
            return (false, new NotFound404Exception(nameof(User), summit.UserId));
        }

        // check if there is already a summit within a set radius (in meters)
        var existingSummits = await _repository.Summit.Index();
        if (existingSummits.Any(s => s.Coordinate!.IsWithinRange(summit.Coordinate!, Summit.SummitProximityRadius)))
        {
            return (false, new BadRequest400Exception($"A summit already exists within a {Summit.SummitProximityRadius}-meter radius."));
        }

        // ensure that the country is valid
        if (!(await _repository.HttpCountry.Index()).ToList().Any(c => c.Name!.Common == summit.Country))
        {
            return (false, new BadRequest400Exception($"A {summit.Country}' is not a valid country. List of all available countries: GET /v1/api/summit/valid-countries"));
        }

        return (true, null); 
    }
}
