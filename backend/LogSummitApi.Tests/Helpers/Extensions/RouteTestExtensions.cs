using Bogus;
using LogSummitApi.Application.Core.Services.Summits.Coordinates;
using LogSummitApi.Domain.Core.Dto.Summits.Routes;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Utilities;

namespace LogSummitApi.Tests.Helpers.Extensions;

public static class RouteTestExtensions
{
    private static readonly Faker<Route> _routeFaker = new Faker<Route>()
        .RuleFor(r => r.Id, f => f.Random.Guid())
        .RuleFor(r => r.SummitId, f => f.Random.Guid())
        .RuleFor(r => r.UserId, f => f.Random.Guid())
        .RuleFor(r => r.Name, f => f.Lorem.Sentence(3))
        .RuleFor(r => r.Description, f => f.Lorem.Paragraph())
        .RuleFor(r => r.IsPublic, _ => true)
        .RuleFor(r => r.Coordinates, f =>
        [
            new Coordinate(f.Address.Latitude(), f.Address.Longitude(), 100),
            new Coordinate(f.Address.Latitude(), f.Address.Longitude(), 200),
            new Coordinate(f.Address.Latitude(), f.Address.Longitude(), 150),
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

        fakeRoute.Distance = fakeRoute.Coordinates.TotalDistance();
        fakeRoute.ElevationGain = fakeRoute.Coordinates.TotalElevationGain();
        fakeRoute.ElevationLoss = fakeRoute.Coordinates.TotalElevationLoss();

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
            IsPublic = route.IsPublic,
            Coordinates = route.Coordinates,
        };
    }
}
