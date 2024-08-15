using LogSummitApi.Domain.Core.Dto.User;
using LogSummitApi.Domain.Core.Utilities;

namespace LogSummitApi.Domain.Core.Dto.Summit.Routes;

public record class RouteDto
{
    public required Guid Id { get; set; }
    public UserDto? User { get; set; }
    public required string? Name { get; set; }
    public required string? Description { get; set; }
    public required double Distance { get; set; }
    public required double ElevationGain { get; set; }
    public required double ElevationLoss { get; set; }
    public SummitDto? Summit { get; set; }
    public required List<Coordinate> Coordinates { get; set; } = [];
    public required DateTime CreatedAt { get; set; }
}
