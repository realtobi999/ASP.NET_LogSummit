using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LogSummitApi.Domain.Core.Dto.Summit;
using LogSummitApi.Domain.Core.Utilities.Coordinates;

namespace LogSummitApi.Domain.Core.Entities;

// TODO: add country property with third party validation
public class Summit
{
    [Required, Column("id")]
    public Guid Id { get; set; }

    [Required, Column("user_id")]
    public Guid UserId { get; set; }

    [Required, Column("name"), MaxLength(155)]
    public string? Name { get; set; }

    [Required, Column("description"), MaxLength(1555)]
    public string? Description { get; set; }

    [Required, Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Required, Column("coordinate"), JsonIgnore]
    public string CoordinateString
    {
        get 
        {
            if (this.Coordinate is null) throw new InvalidOperationException("Coordinate cannot be null.");

            return this.Coordinate.ToString();
        }
        set
        {   
            Coordinate = Coordinate.Parse(value);
        }
    }

    public const double SummitProximityRadius = 55; // in this radius (in meters) no other summit can be located

    // entity relationships

    [NotMapped]
    public Coordinate? Coordinate { get; set; }

    public User? User { get; set; }

    // methods

    public SummitDto ToDto()
    {
        return new SummitDto()
        {
            Id = this.Id,
            UserId = this.UserId,
            Name = this.Name,
            Description = this.Description,
            CreatedAt = this.CreatedAt,
            Coordinate = this.Coordinate,
        };
    }
}
