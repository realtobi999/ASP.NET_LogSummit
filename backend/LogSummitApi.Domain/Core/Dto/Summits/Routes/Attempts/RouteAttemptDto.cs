using GeoCoordinates.Core;
using LogSummitApi.Domain.Core.Dto.Users;

namespace LogSummitApi.Domain.Core.Dto.Summits.Routes.Attempts;

public record class RouteAttemptDto
{
    public required Guid Id { get; set; }
    public UserDto? User { get; set; }
    public required string? Name { get; set; }
    public required string? Description { get; set; }
    public required bool IsPublic { get; set; }
    public required TimeSpan Time { get; set; }
    public required List<Coordinate> Coordinates { get; set; }
    public RouteDto? Route { get; set; }
    public required DateTime CreatedAt { get; set; }
}
