using LogSummitApi.Domain.Core.Dto.Summit.Routes;
using LogSummitApi.Domain.Core.Dto.User;
using LogSummitApi.Domain.Core.Utilities;

namespace LogSummitApi.Domain.Core.Dto.Summit;

public record class SummitDto
{
    public required Guid Id { get; set; }
    public UserDto? User { get; set; }
    public required string? Name { get; set; }
    public required string? Description { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required Coordinate? Coordinate { get; set; }
    public List<RouteDto>? Routes { get; set; }
}
