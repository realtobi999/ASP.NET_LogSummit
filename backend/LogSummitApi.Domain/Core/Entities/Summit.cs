using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LogSummitApi.Domain.Core.Attributes;
using LogSummitApi.Domain.Core.Dto.Summit;
using LogSummitApi.Domain.Core.Interfaces.Common;
using LogSummitApi.Domain.Core.Utilities.Coordinates;

namespace LogSummitApi.Domain.Core.Entities;

public class Summit : ISerializable<SummitDto>
{
    [Required, Column("id")]
    public Guid Id { get; set; }

    [Required, Column("user_id")]
    public Guid UserId { get; set; }

    [Required, Column("name"), MaxLength(155)]
    public string? Name { get; set; }

    [Required, Column("description"), MaxLength(1555)]
    public string? Description { get; set; }

    [Required, MaxLength(155)]
    public string? Country { get; set; }

    [Required, Column("is_public")]
    public bool IsPublic { get; set; }

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
    public const double RouteRadius = 10;

    // entity relationships

    [NotMapped]
    public Coordinate? Coordinate { get; set; }

    [IncludeInQuerying]
    public User? User { get; set; }
    public ICollection<Route>? Routes { get; set; } 

    // methods

    public SummitDto? ToDto()
    {
        if (this.User is null) throw new NullReferenceException("Summit 'User' property cannot be null.");

        if (!this.IsPublic)
        {
            return null;
        }

        return new SummitDto()
        {
            Id = this.Id,
            User = this.User.ToDto(),
            Name = this.Name,
            Description = this.Description,
            CreatedAt = this.CreatedAt,
            Coordinate = this.Coordinate,
        };
    }
}
