using GeoCoordinates.Core;
using LogSummitApi.Domain.Core.Dto.Users;

namespace LogSummitApi.Domain.Core.Dto.Summits.Routes;

public record class RouteDto
{
    public required Guid Id { get; init; }
    public UserDto? User { get; init; }
    public required string? Name { get; init; }
    public required string? Description { get; init; }
    public required double Distance { get; init; }
    public required double ElevationGain { get; init; }
    public required double ElevationLoss { get; init; }
    public SummitDto? Summit { get; init; }
    public required List<Coordinate> Coordinates { get; init; } = [];
    public required DateTime CreatedAt { get; init; }
}
