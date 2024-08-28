using System.ComponentModel.DataAnnotations;
using GeoCoordinates.Core;

namespace LogSummitApi.Domain.Core.Dto.Summits.Routes.Attempts;

public record class CreateRouteAttemptDto
{
    public Guid? Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid SummitId { get; set; }

    [Required]
    public Guid RouteId { get; set; }

    [Required, MaxLength(155)]
    public string? Name { get; set; }

    [Required, MaxLength(1555)]
    public string? Description { get; set; }

    [Required]
    public bool? IsPublic { get; set; }

    [Required, MinLength(2)]
    public List<Coordinate> Coordinates { get; set; } = [];

    [Required]
    public TimeSpan Time { get; set; }
}
