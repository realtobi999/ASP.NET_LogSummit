using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LogSummitApi.Domain.Core.Dto.Summit;
using LogSummitApi.Domain.Core.Dto.Summit.Routes;
using LogSummitApi.Domain.Core.Exceptions.Common;
using LogSummitApi.Domain.Core.Interfaces.Common;
using LogSummitApi.Domain.Core.Utilities;

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

    public const double SummitProximityRadius = 55; // in this radius around the summit (in meters) no other summit can be located
    public const double RouteProximityRadius = 10; // in this radius around the summit (in meters) the route last coordinate must be

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
            Routes = this.Routes.Select(r => new RouteDto()
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
