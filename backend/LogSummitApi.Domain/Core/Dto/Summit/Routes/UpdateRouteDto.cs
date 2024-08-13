using System.ComponentModel.DataAnnotations;
using LogSummitApi.Domain.Core.Utilities.Coordinates;

namespace LogSummitApi.Domain.Core.Dto.Summit.Routes;

public record class UpdateRouteDto
{
    [Required]
    public Guid SummitId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required, MaxLength(155)]
    public string? Name { get; set; }

    [Required, MaxLength(1555)]
    public string? Description { get; set; }

    [Required, Range(0, double.MaxValue)]
    public double Distance { get; set; }

    [Required, Range(0, double.MaxValue)]
    public double ElevationGain { get; set; }

    [Required, Range(0, double.MaxValue)]
    public double ElevationLoss { get; set; }

    [Required, MinLength(2)]
    public List<Coordinate>? Coordinates { get; set; }
}
