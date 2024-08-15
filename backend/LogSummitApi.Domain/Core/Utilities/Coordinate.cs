using LogSummitApi.Domain.Core.Exceptions.Common;

namespace LogSummitApi.Domain.Core.Utilities;

public class Coordinate
{
    public const string StringCoordinateFormat = "latitude|longitude|elevation";
    public const double MaximumElevationPoint = 8848;
    public const double LowestElevationPoint = -420;

    public double Latitude { get; }
    public double Longitude { get; }
    public double Elevation { get; } // in meters

    public Coordinate(double latitude, double longitude, double elevation)
    {
        if (latitude < -90 || latitude > 90)
        {
            throw new InvalidCoordinateFormatException(this, "Latitude must be between -90 and 90 degrees.");
        }

        if (longitude < -180 || longitude > 180)
        {
            throw new InvalidCoordinateFormatException(this, "Longitude must be between -180 and 180 degrees.");
        }

        if (LowestElevationPoint >= elevation || elevation >= MaximumElevationPoint)
        {
            throw new InvalidCoordinateFormatException(this, $"Elevation must be between {LowestElevationPoint} meters and {MaximumElevationPoint} meters.");
        }

        Latitude = latitude;
        Longitude = longitude;
        Elevation = elevation;
    }

    public override string ToString()
    {
        return $"{Latitude}|{Longitude}|{Elevation}";
    }

    public static Coordinate Parse(string coordinate)
    {
        var coordinates = coordinate.Split('|');
        if (coordinates.Length != 3)
        {
            throw new InvalidCoordinateFormatException($"Invalid coordinate format. Expected format: '{StringCoordinateFormat}'");
        }

        if (!double.TryParse(coordinates[0], out double latitude))
        {
            throw new InvalidCoordinateFormatException("Invalid latitude value.");
        }
        if (!double.TryParse(coordinates[1], out double longitude))
        {
            throw new InvalidCoordinateFormatException("Invalid longitude value.");
        }
        if (!double.TryParse(coordinates[2], out double elevation))
        {
            throw new InvalidCoordinateFormatException("Invalid elevation value.");
        }

        return new Coordinate(latitude, longitude, elevation);
    }
}
