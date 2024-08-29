using System.ComponentModel.DataAnnotations;
using GeoCoordinates.Core;

namespace LogSummitApi.Domain.Core.Dto.Summits;

public record class UpdateSummitDto
{
    [Required, MinLength(3), MaxLength(155)]
    public string? Name { get; init; }

    [Required, MinLength(15), MaxLength(1555)]
    public string? Description { get; init; }

    [Required, MaxLength(155)]
    public string? Country { get; init; }

    [Required]
    public bool? IsPublic { get; init; }

    [Required]
    public Coordinate? Coordinate { get; init; }
}