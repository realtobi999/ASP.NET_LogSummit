using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.Http;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Domain.Core.Interfaces.Utilities;

namespace LogSummitApi.Application.Core.Validators;

public class RouteValidator : IValidator<Route>
{
    private readonly IRepositoryManager _repository;

    public RouteValidator(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<(bool isValid, Exception? exception)> IsValidAsync(Route route)
    {
        // validate that the user exists
        if (await _repository.Users.GetAsync(route.UserId) is null)
        {
            return (false, new NotFound404Exception(nameof(User), route.UserId));
        }

        // validate that the summit exists
        var summit = await _repository.Summit.GetAsync(route.SummitId);
        if (summit is null)
        {
            return (false, new NotFound404Exception(nameof(Summit), route.SummitId));
        }

        // validate that the last coordinate is atleast 10 meters in the range of the summit coordinate
        if (summit.Coordinate is not null && !route.Coordinates.Last().IsWithinRange(summit.Coordinate, Summit.RouteRadius))
        {
            return (false, new BadRequest400Exception($"The last coordinate of the summit route is not within a {Summit.RouteRadius}-meter range of the summit coordinate."));
        }

        return (true, null);
    }
}
