using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Utilities;

namespace LogSummitApi.Application.Core.Services.Summits.Coordinates;

public static class CoordinateExtensions
{
    public static double TotalDistance(this IEnumerable<Coordinate> coordinates)
    {
        var distance = 0.0; // in meters

        for (int i = 0; i < coordinates.Count() - 1; i++)
        {
            distance += CoordinateMath.Haversine(coordinates.ElementAt(i), coordinates.ElementAt(i + 1));
        }

        return distance;
    }

    public static bool IsAlignedWith(this IEnumerable<Coordinate> routeCoordinates, IEnumerable<Coordinate> attemptCoordinates, double allowedDeviation)
    {
        return CoordinateHelpers.ValidateCoordinatePathAlignments(routeCoordinates, attemptCoordinates, allowedDeviation);
    }

    public static double TotalElevationGain(this IEnumerable<Coordinate> coordinates)
    {
        return CoordinateHelpers.CalculateElevationChange(coordinates, 5, 5).gain;
    }

    public static double TotalElevationLoss(this IEnumerable<Coordinate> coordinates)
    {
        return CoordinateHelpers.CalculateElevationChange(coordinates, 5, 5).loss;
    }

    public static bool IsWithinRange(this Coordinate coordinate1, Coordinate coordinate2, double range)
    {
        return CoordinateHelpers.AreWithinRange(coordinate1, coordinate2, range);
    }
}

