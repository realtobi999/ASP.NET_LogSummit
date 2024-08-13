using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LogSummitApi.Domain.Core.Utilities.Coordinates;

namespace LogSummitApi.Domain.Core.Dto.Summit;

public record class CreateSummitDto
{
    public Guid? Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required, MaxLength(155)]
    public string? Name { get; set; }

    [Required, MaxLength(1555)]
    public string? Description { get; set; }

    [Required, MaxLength(155)]
    public string? Country { get; set; }

    [Required]
    public bool? IsPublic { get; set; }

    [Required]
    public Coordinate? Coordinate { get; set; }
}
