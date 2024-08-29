using System.ComponentModel.DataAnnotations;
using GeoCoordinates.Core;

namespace LogSummitApi.Domain.Core.Dto.Summits.Routes;

public record class UpdateRouteDto
{
    [Required, MinLength(3), MaxLength(155)]
    public string? Name { get; init; }

    [Required, MinLength(15), MaxLength(1555)]
    public string? Description { get; init; }

    [Required]
    public bool? IsPublic { get; init; }

    [Required, MinLength(2)]
    public List<Coordinate> Coordinates { get; init; } = [];
}
