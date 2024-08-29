using System.ComponentModel.DataAnnotations;
using GeoCoordinates.Core;

namespace LogSummitApi.Domain.Core.Dto.Summits.Routes.Attempts;

public record class CreateRouteAttemptDto
{
    public Guid? Id { get; init; }

    [Required]
    public Guid UserId { get; init; }

    [Required]
    public Guid SummitId { get; init; }

    [Required]
    public Guid RouteId { get; init; }

    [Required, MaxLength(155)]
    public string? Name { get; init; }

    [Required, MaxLength(1555)]
    public string? Description { get; init; }

    [Required]
    public bool? IsPublic { get; init; }

    [Required, MinLength(2)]
    public List<Coordinate> Coordinates { get; init; } = [];

    [Required]
    public TimeSpan Time { get; init; }
}
