using Bogus;
using LogSummitApi.Domain.Core.Dto.Summits.Routes.Attempts;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Utilities;

namespace LogSummitApi.Tests.Helpers.Extensions;

public static class RouteAttemptTestExtensions
{
    private static readonly Faker<RouteAttempt> _routeAttemptFaker = new Faker<RouteAttempt>()
        .RuleFor(ra => ra.Id, f => f.Random.Guid())
        .RuleFor(ra => ra.UserId, f => f.Random.Guid())
        .RuleFor(ra => ra.SummitId, f => f.Random.Guid())
        .RuleFor(ra => ra.RouteId, f => f.Random.Guid())
        .RuleFor(ra => ra.Name, f => f.Lorem.Sentence(3))
        .RuleFor(ra => ra.Description, f => f.Lorem.Paragraph())
        .RuleFor(ra => ra.IsPublic, _ => true)
        .RuleFor(ra => ra.Time, f => f.Date.Timespan())
        .RuleFor(ra => ra.Coordinates, f =>
        [
            new Coordinate(f.Address.Latitude(), f.Address.Longitude(), 100),
            new Coordinate(f.Address.Latitude(), f.Address.Longitude(), 200),
            new Coordinate(f.Address.Latitude(), f.Address.Longitude(), 150),
        ])
        .RuleFor(ra => ra.CreatedAt, _ => DateTime.UtcNow);

    public static RouteAttempt WithFakeData(this RouteAttempt _, User user, Route route)
    {
        var fakeRouteAttempt = _routeAttemptFaker.Generate();

        fakeRouteAttempt.Coordinates = route.Coordinates;
        fakeRouteAttempt.UserId = user.Id;
        fakeRouteAttempt.RouteId = route.Id;
        fakeRouteAttempt.SummitId = route.SummitId;
        fakeRouteAttempt.User = user;
        fakeRouteAttempt.Route = route;

        // Ensure that the last coordinate is the summit coordinate
        if (route.Summit?.Coordinate != null && fakeRouteAttempt.Coordinates.Count > 0)
        {
            fakeRouteAttempt.Coordinates[^1] = route.Summit.Coordinate;
        }

        return fakeRouteAttempt;
    }

    public static CreateRouteAttemptDto ToCreateRouteAttemptDto(this RouteAttempt attempt)
    {
        return new CreateRouteAttemptDto
        {
            Id = attempt.Id,
            UserId = attempt.UserId,
            SummitId = attempt.SummitId,
            RouteId = attempt.RouteId,
            Name = attempt.Name,
            Description = attempt.Description,
            IsPublic = attempt.IsPublic,
            Time = attempt.Time,
            Coordinates = attempt.Coordinates
        };
    }

}
