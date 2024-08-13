using Bogus;
using LogSummitApi.Domain.Core.Dto.Summit.Routes;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Utilities.Coordinates;

namespace LogSummitApi.Tests.Helpers.Extensions;

public static class RouteTestExtensions
{
    private static readonly Faker<Route> _routeFaker = new Faker<Route>()
        .RuleFor(sp => sp.Id, f => f.Random.Guid())
        .RuleFor(sp => sp.SummitId, f => f.Random.Guid())
        .RuleFor(sp => sp.UserId, f => f.Random.Guid())
        .RuleFor(sp => sp.Name, f => f.Lorem.Sentence(3))
        .RuleFor(sp => sp.Description, f => f.Lorem.Paragraph())
        .RuleFor(sp => sp.Distance, f => f.Random.Double(0, 10000))
        .RuleFor(sp => sp.ElevationGain, f => f.Random.Double(0, 10000))
        .RuleFor(sp => sp.ElevationLoss, f => f.Random.Double(0, 10000))
        .RuleFor(sp => sp.Coordinates, f =>
        [
            new Coordinate(f.Address.Latitude(), f.Address.Longitude(), 100),
            new Coordinate(f.Address.Latitude(), f.Address.Longitude(), 200)
        ])
        .RuleFor(sp => sp.CreatedAt, _ => DateTime.UtcNow);

    public static Route WithFakeData(this Route _, User user, Summit summit)
    {
        var fakeRoute = _routeFaker.Generate();

        fakeRoute.UserId = user.Id;
        fakeRoute.SummitId = summit.Id;
        fakeRoute.User = user;
        fakeRoute.Summit = summit;

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
            Coordinates = route.Coordinates,
        };
    }
}
