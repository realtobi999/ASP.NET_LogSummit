using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LogSummitApi.Domain.Core.Dto.Summits;
using LogSummitApi.Domain.Core.Dto.Summits.Routes;
using LogSummitApi.Domain.Core.Exceptions.Common;
using LogSummitApi.Domain.Core.Interfaces.Common;
using LogSummitApi.Domain.Core.Utilities;

namespace LogSummitApi.Domain.Core.Entities;

public class Route : ISerializable<RouteDto>
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

    [Required, Column("is_public")]
    public bool IsPublic { get; set; }

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
    public ICollection<RouteAttempt>? RouteAttempts { get; set; }
    public User? User { get; set; }
    public Summit? Summit { get; set; }

    // methods

    public RouteDto ToDto()
    {
        if (this.Summit is null) throw new NullPropertyException(nameof(Route), nameof(Summit));
        if (this.User is null) throw new NullPropertyException(nameof(Route), nameof(User));

        return new RouteDto()
        {
            Id = this.Id,
            User = this.User.ToDto(),
            Name = this.Name,
            Description = this.Description,
            Distance = this.Distance,
            ElevationGain = this.ElevationGain,
            ElevationLoss = this.ElevationLoss,
            Summit = new SummitDto
            {
                Id = this.Summit.Id,
                Name = this.Summit.Name,
                Description = this.Summit.Description,
                CreatedAt = this.Summit.CreatedAt,
                Coordinate = this.Summit.Coordinate,
            },
            Coordinates = this.Coordinates,
            CreatedAt = this.CreatedAt,
        };
    }
}


