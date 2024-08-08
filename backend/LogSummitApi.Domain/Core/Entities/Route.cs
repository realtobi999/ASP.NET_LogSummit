using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LogSummitApi.Domain.Core.Attributes;
using LogSummitApi.Domain.Core.Dto.Summit.Routes;
using LogSummitApi.Domain.Core.Utilities.Coordinates;

namespace LogSummitApi.Domain.Core.Entities;

public class Route
{
    [Required, Column("id")]
    public Guid Id { get; set; }

    [Required, Column("summit_id")]
    public Guid SummitId { get; set; }

    [Required, Column("user_id")]
    public Guid UserId { get; set; }

    [Required, Column("name"), MaxLength(155)]
    public string? Name { get; set; }

    [Required, Column("description"), MaxLength(1555)]
    public string? Description { get; set; }

    [Required, Column("distance"), Range(0, double.MaxValue)]
    public double Distance { get; set; }

    [Required, Column("elevation_gain"), Range(0, double.MaxValue)]
    public double ElevationGain { get; set; }

    [Required, Column("elevation_loss"), Range(0, double.MaxValue)]
    public double ElevationLoss { get; set; }

    [Required, Column("coordinates"), JsonIgnore]
    public string CoordinatesString
    {
        get => string.Join(";", Coordinates.Select(a => a.ToString()));
        set => Coordinates = value.Split(';').Select(Coordinate.Parse).ToList();
    }
    
    [Required, Column("created_at")]
    public DateTime CreatedAt { get; set; }

    // entity relationships

    [NotMapped]
    public List<Coordinate> Coordinates = [];

    [IncludeInQuerying]
    public User? User { get; set; }
    
    [IncludeInQuerying]
    public Summit? Summit { get; set; }

    // methods

    public RouteDto ToDto()
    {
        if (this.Summit is null) throw new NullReferenceException("Route 'Summit' property cannot be null.");
        if (this.User is null) throw new NullReferenceException("Route 'User' property cannot be null.");
    
        return new RouteDto()
        {
            Id = this.Id,
            Summit = this.Summit.ToDto(),
            User = this.User.ToDto(),
            Name = this.Name,
            Description = this.Description,
            Distance = this.Distance,
            ElevationGain = this.ElevationGain,
            ElevationLoss = this.ElevationLoss,
            Coordinates = this.Coordinates,
            CreatedAt = this.CreatedAt,
        };
    }
}


