using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LogSummitApi.Domain.Core.Utilities.Coordinates;

namespace LogSummitApi.Domain.Core.Entities;

public class SummitPush
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

    [Required, Column("max_speed"), Range(0, double.MaxValue)]
    public double MaxSpeed { get; set; }

    [Required, Column("moving_time")]
    public TimeSpan MovingTime { get; set; }

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
    public Summit? Summit { get; set; }
}


