using Bogus;
using LogSummitApi.Domain.Core.Dto.Summit.Routes;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Utilities.Coordinates;

namespace LogSummitApi.Tests.Helpers.Extensions;

public static class RouteTestExtensions
{
    private static readonly Faker<Route> _routeFaker = new Faker<Route>()
        .RuleFor(r => r.Id, f => f.Random.Guid())
        .RuleFor(r => r.SummitId, f => f.Random.Guid())
        .RuleFor(r => r.UserId, f => f.Random.Guid())
        .RuleFor(r => r.Name, f => f.Lorem.Sentence(3))
        .RuleFor(r => r.Description, f => f.Lorem.Paragraph())
        .RuleFor(r => r.Distance, f => f.Random.Double(0, 10000))
        .RuleFor(r => r.ElevationGain, f => f.Random.Double(0, 10000))
        .RuleFor(r => r.ElevationLoss, f => f.Random.Double(0, 10000))
        .RuleFor(r => r.IsPublic, _ => true)
        .RuleFor(r => r.Coordinates, f =>
        [
            new Coordinate(f.Address.Latitude(), f.Address.Longitude(), 100),
            new Coordinate(f.Address.Latitude(), f.Address.Longitude(), 200)
        ])
        .RuleFor(r => r.CreatedAt, _ => DateTime.UtcNow);

    public static Route WithFakeData(this Route _, User user, Summit summit)
    {
        var fakeRoute = _routeFaker.Generate();

        fakeRoute.UserId = user.Id;
        fakeRoute.SummitId = summit.Id;
        fakeRoute.User = user;
        fakeRoute.Summit = summit;

        // ensure that the last coordinate is the summit coordinate
        if (summit.Coordinate is not null && fakeRoute.Coordinates.Count > 0)
        {
            fakeRoute.Coordinates[^1] = summit.Coordinate;
        }

        return fakeRoute;
    }

    public static CreateRouteDto ToCreateRouteDto(this Route route)
    {
        return new CreateRouteDto()
        {
            Id = route.Id,
            UserId = route.UserId,
            SummitId = route.SummitId,
            Name = route.Name,
            Description = route.Description,
            Distance = route.Distance,
            ElevationGain = route.ElevationGain,
            ElevationLoss = route.ElevationLoss,
            IsPublic = route.IsPublic,
            Coordinates = route.Coordinates,
        };
    }
}
