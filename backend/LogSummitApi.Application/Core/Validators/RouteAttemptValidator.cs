using LogSummitApi.Application.Core.Services.Summits.Coordinates;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.Http;
using LogSummitApi.Domain.Core.Interfaces.Common;
using LogSummitApi.Domain.Core.Interfaces.Repositories;

namespace LogSummitApi.Application.Core.Validators;

public class RouteAttemptValidator : IValidator<RouteAttempt>
{
    private readonly IRepositoryManager _repository;

    public RouteAttemptValidator(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<(bool isValid, Exception? exception)> IsValidAsync(RouteAttempt attempt)
    {
        // validate that the user exists
        if (await _repository.Users.GetAsync(attempt.UserId) is null)
        {
            return (false, new NotFound404Exception(nameof(User), attempt.UserId));
        }

        // validate that the summit exists
        var summit = await _repository.Summit.GetAsync(attempt.SummitId);
        if (summit is null)
        {
            return (false, new NotFound404Exception(nameof(Summit), attempt.SummitId));
        }

        // validate that the summit exists
        var route = await _repository.Route.GetAsync(attempt.RouteId);
        if (route is null)
        {
            return (false, new NotFound404Exception(nameof(Route), attempt.RouteId));
        }

        if (!route.IsPublic && attempt.UserId == route.UserId)
        {
            return (false, new NotAuthorized401Exception());
        }

        // ensure that the right visibility is enforced
        if (!route.IsPublic && attempt.IsPublic)
        {
            return (false, new BadRequest400Exception($"Cannot set the route attempt to public because the route is private. Please set the summit to public or the route to private."));
        } 

        // validate that the first coordinate is atleast 10 meters in the range of the starting route coordinate
        if (!attempt.Coordinates.First().HasInRange(route.Coordinates.First(), Route.FIRST_COORDINATE_TOLERANCE_RADIUS))
        {
            return (false, new BadRequest400Exception($"The first coordinate of the route attempt is not withing a {Route.FIRST_COORDINATE_TOLERANCE_RADIUS}-meter range of the fist route coordinate"));
        }

        // validate that the last coordinate is atleast 10 meters in the range of the summit coordinate
        if (summit.Coordinate is not null && !attempt.Coordinates.Last().HasInRange(summit.Coordinate, Summit.FINAL_COORDINATE_TOLERANCE_RADIUS))
        {
            return (false, new BadRequest400Exception($"The last coordinate of the route attempt is not within a {Summit.FINAL_COORDINATE_TOLERANCE_RADIUS}-meter range of the summit coordinate."));
        }

        // validate that the route and attempt coordinates are in aliment relative to the route coordinates
        if (!route.Coordinates.IsAlignedWith(attempt.Coordinates, Route.ALLOWED_DEVIATION_RADIUS))
        {
            return (false, new BadRequest400Exception($"The route attempt does not align with the route coordinates within the allowed deviation of {Route.ALLOWED_DEVIATION_RADIUS} meters."));
        }

        return (true, null); 
    }
}
