using System.ComponentModel.DataAnnotations;
using LogSummitApi.Domain.Core.Utilities;

namespace LogSummitApi.Domain.Core.Dto.Summit.Routes;

public record class UpdateRouteDto
{
    [Required, MinLength(3), MaxLength(155)]
    public string? Name { get; set; }

    [Required, MinLength(15), MaxLength(1555)]
    public string? Description { get; set; }

    [Required]
    public bool? IsPublic { get; set; }

    [Required, MinLength(2)]
    public List<Coordinate> Coordinates { get; set; } = [];
}
