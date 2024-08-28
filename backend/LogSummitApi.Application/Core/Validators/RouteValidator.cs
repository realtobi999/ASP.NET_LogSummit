using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.Http;
using LogSummitApi.Domain.Core.Interfaces.Common;
using LogSummitApi.Domain.Core.Interfaces.Repositories;

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

        // ensure that if the summit is private only the owner can create a route
        if (!summit.IsPublic && route.UserId != summit.UserId)
        {
            return (false, new NotAuthorized401Exception());
        }

        // ensure the right visibility is enforced
        if (!summit.IsPublic && route.IsPublic)
        {
            return (false, new BadRequest400Exception($"Cannot set the route to public because the summit is private. Please set the summit to public or the route to private."));
        }

        // validate that the last coordinate is atleast 10 meters in the range of the summit coordinate
        if (summit.Coordinate is not null && !route.Coordinates.Last().IsWithinDistanceTo(summit.Coordinate, Summit.FINAL_COORDINATE_TOLERANCE_RADIUS))
        {
            return (false, new BadRequest400Exception($"The last coordinate of the summit route is not within a {Summit.FINAL_COORDINATE_TOLERANCE_RADIUS}-meter range of the summit coordinate."));
        }

        return (true, null);
    }
}
