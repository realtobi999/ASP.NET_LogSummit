using LogSummitApi.Domain.Core.Dto.User;
using LogSummitApi.Domain.Core.Utilities;

namespace LogSummitApi.Domain.Core.Dto.Summit;

public record class SummitDto
{
    public Guid Id { get; set; }
    public UserDto? User { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public Coordinate? Coordinate { get; set; }
}
