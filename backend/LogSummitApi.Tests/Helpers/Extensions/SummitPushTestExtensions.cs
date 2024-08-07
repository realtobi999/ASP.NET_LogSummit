using Bogus;
using LogSummitApi.Domain.Core.Dto.Summit.Pushes;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Utilities.Coordinates;

namespace LogSummitApi.Tests.Helpers.Extensions;

public static class SummitPushTestExtensions
{
    private static readonly Faker<SummitPush> _summitPushFaker = new Faker<SummitPush>()
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

    public static SummitPush WithFakeData(this SummitPush _, User user, Summit summit)
    {
        var fakeSummitPush = _summitPushFaker.Generate();

        fakeSummitPush.UserId = user.Id;
        fakeSummitPush.SummitId = summit.Id;
        fakeSummitPush.User = user;
        fakeSummitPush.Summit = summit;

        if (summit.Coordinate is not null && fakeSummitPush.Coordinates.Count > 0)
        {
            fakeSummitPush.Coordinates[^1] = summit.Coordinate;
        }

        return fakeSummitPush;
    }

    public static CreateSummitPushDto ToCreateSummitPushDto(this SummitPush summitPush)
    {
        return new CreateSummitPushDto()
        {
            Id = summitPush.Id,
            UserId = summitPush.UserId,
            SummitId = summitPush.SummitId,
            Name = summitPush.Name,
            Description = summitPush.Description,
            Distance = summitPush.Distance,
            ElevationGain = summitPush.ElevationGain,
            ElevationLoss = summitPush.ElevationLoss,
            Coordinates = summitPush.Coordinates,
        };
    }
}
