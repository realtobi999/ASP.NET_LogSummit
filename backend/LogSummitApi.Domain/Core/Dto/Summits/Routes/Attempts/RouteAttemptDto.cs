using GeoCoordinates.Core;
using LogSummitApi.Domain.Core.Dto.Users;

namespace LogSummitApi.Domain.Core.Dto.Summits.Routes.Attempts;

public record class RouteAttemptDto
{
    public required Guid Id { get; init; }
    public UserDto? User { get; init; }
    public required string? Name { get; init; }
    public required string? Description { get; init; }
    public required bool IsPublic { get; init; }
    public required TimeSpan Time { get; init; }
    public required List<Coordinate> Coordinates { get; init; }
    public RouteDto? Route { get; init; }
    public required DateTime CreatedAt { get; init; }
}
