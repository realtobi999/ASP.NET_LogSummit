using LogSummitApi.Domain.Core.Dto.User;
using LogSummitApi.Domain.Core.Utilities.Coordinates;

namespace LogSummitApi.Domain.Core.Dto.Summit.Routes;

public record class RouteDto
{
    public Guid Id { get; set; }
    public SummitDto? Summit { get; set; }
    public UserDto? User { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public double Distance { get; set; }
    public double ElevationGain { get; set; }
    public double ElevationLoss { get; set; }
    public double AverageSpeed { get; set; }
    public List<Coordinate> Coordinates { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}
