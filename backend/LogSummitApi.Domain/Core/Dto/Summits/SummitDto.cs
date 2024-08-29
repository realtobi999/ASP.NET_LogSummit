using GeoCoordinates.Core;
using LogSummitApi.Domain.Core.Dto.Summits.Routes;
using LogSummitApi.Domain.Core.Dto.Users;

namespace LogSummitApi.Domain.Core.Dto.Summits;

public record class SummitDto
{
    public required Guid Id { get; init; }
    public UserDto? User { get; init; }
    public required string? Name { get; init; }
    public required string? Description { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required Coordinate? Coordinate { get; init; }
    public List<RouteDto>? Routes { get; init; }
}
