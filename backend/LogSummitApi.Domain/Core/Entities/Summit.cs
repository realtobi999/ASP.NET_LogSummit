using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LogSummitApi.Domain.Core.Dto.Summits.Routes;
using LogSummitApi.Domain.Core.Dto.Summits;
using LogSummitApi.Domain.Core.Exceptions.Common;
using LogSummitApi.Domain.Core.Interfaces.Common;
using GeoCoordinates.Core;

namespace LogSummitApi.Domain.Core.Entities;

public class Summit : ISerializable<SummitDto>
{
    [Required, Column("id")]
    public Guid Id { get; set; }

    [Required, Column("user_id")]
    public Guid UserId { get; set; }

    [Required, Column("name")]
    public string? Name { get; set; }

    [Required, Column("description")]
    public string? Description { get; set; }

    [Required, Column("country")]
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
            if (this.Coordinate is null) throw new NullPropertyException(nameof(Summit), nameof(Coordinate));

            return this.Coordinate.ToString();
        }
        set => Coordinate = Coordinate.Parse(value);
    }

    // The minimum required radius (in meters) around the summit within which no other summit can be located. 
    // This ensures that summits are adequately spaced apart to avoid overlapping.
    public const double SUMMIT_PROXIMITY_RADIUS = 55;

    // The maximum allowed radius (in meters) within which the final coordinate of the Route must be 
    // located relative to the summit's coordinate. This ensures that the end of the RouteAttempt 
    // is close enough to the summit, accurately marking the completion of the route.
    public const double FINAL_COORDINATE_TOLERANCE_RADIUS = 10;


    // entity relationships

    [NotMapped]
    public Coordinate? Coordinate { get; set; }
    public User? User { get; set; }
    public ICollection<Route>? Routes { get; set; }

    // methods

    public SummitDto? ToDto()
    {
        if (this.User is null) throw new NullPropertyException(nameof(Summit), nameof(User));
        if (this.Routes is null) throw new NullPropertyException(nameof(Summit), nameof(Routes));

        return new SummitDto()
        {
            Id = this.Id,
            User = this.User.ToDto(),
            Name = this.Name,
            Description = this.Description,
            CreatedAt = this.CreatedAt,
            Coordinate = this.Coordinate,
            Routes = this.Routes.Where(r => r.IsPublic).Select(r => new RouteDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Distance = r.Distance,
                ElevationGain = r.ElevationGain,
                ElevationLoss = r.ElevationLoss,
                Coordinates = r.Coordinates,
                CreatedAt = r.CreatedAt,
            }).ToList(),
        };
    }
}
