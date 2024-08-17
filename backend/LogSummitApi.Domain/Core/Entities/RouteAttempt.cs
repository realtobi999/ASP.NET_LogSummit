using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LogSummitApi.Domain.Core.Dto.Summits;
using LogSummitApi.Domain.Core.Dto.Summits.Routes;
using LogSummitApi.Domain.Core.Dto.Summits.Routes.Attempts;
using LogSummitApi.Domain.Core.Exceptions.Common;
using LogSummitApi.Domain.Core.Interfaces.Common;
using LogSummitApi.Domain.Core.Utilities;

namespace LogSummitApi.Domain.Core.Entities;

public class RouteAttempt : ISerializable<RouteAttemptDto>
{
    [Required, Column("id")]
    public Guid Id { get; set; }

    [Required, Column("user_id")]
    public Guid UserId { get; set; }

    [Required, Column("summit_id")]
    public Guid SummitId { get; set; }

    [Required, Column("route_id")]
    public Guid RouteId { get; set; }

    [Required, Column("name")]
    public string? Name { get; set; }

    [Required, Column("description")]
    public string? Description { get; set; }

    [Required, Column("is_public")]
    public bool IsPublic { get; set; }

    [Required, Column("time")]
    public TimeSpan Time { get; set; }

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
    public User? User { get; set; }
    public Route? Route { get; set; }

    // methods

    public RouteAttemptDto ToDto()
    {
        if (this.User is null) throw new NullPropertyException(nameof(RouteAttempt), nameof(User));
        if (this.Route is null) throw new NullPropertyException(nameof(RouteAttempt), nameof(Route));
        if (this.Route.Summit is null) throw new NullPropertyException(nameof(RouteAttempt), nameof(Route.Summit));

        return new RouteAttemptDto
        {
            Id = this.Id,
            User = this.User.ToDto(),
            Name = this.Name,
            Description = this.Description,
            IsPublic = this.IsPublic,
            Time = this.Time,
            Coordinates = this.Coordinates,
            Route = new RouteDto
            {
                Id = this.Route.Id,
                Name = this.Route.Name,
                Description = this.Route.Description,
                Distance = this.Route.Distance,
                ElevationGain = this.Route.ElevationGain,
                ElevationLoss = this.Route.ElevationLoss,
                Summit = new SummitDto
                {
                    Id = this.Route.Summit.Id,
                    Name = this.Route.Summit.Name,
                    Description = this.Route.Summit.Description,
                    CreatedAt = this.Route.Summit.CreatedAt,
                    Coordinate = this.Route.Summit.Coordinate,
                },
                Coordinates = this.Route.Coordinates,
                CreatedAt = this.Route.CreatedAt,
            },
            CreatedAt = this.CreatedAt,
        };
    }
}
