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

    /// <summary>
    /// Checks whether the sequence of attempt coordinates is aligned with the sequence of route coordinates,
    /// allowing for a specified deviation in distance between corresponding coordinates.
    /// </summary>
    /// <param name="routeCoordinates">The sequence of route coordinates to align against.</param>
    /// <param name="attemptCoordinates">The sequence of attempt coordinates to be validated for alignment.</param>
    /// <param name="allowedDeviation">The maximum allowable distance deviation between corresponding coordinates in the two sequences.</param>
    /// <returns><c>true</c> if each route coordinate has at least one corresponding attempt coordinate within the allowed deviation; otherwise, <c>false</c>.</returns>
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


    /// <summary>
    /// Determines whether the specified coordinate is within the given distance range from the current coordinate.
    /// </summary>
    /// <param name="coordinate1">The current coordinate (this).</param>
    /// <param name="coordinate2">The coordinate to check against the range.</param>
    /// <param name="range">The maximum distance range within which the second coordinate should fall relative to the first coordinate.</param>
    /// <returns><c>true</c> if the coordinate is within the specified range from the base coordinate; otherwise, <c>false</c>.</returns>
    public static bool HasInRange(this Coordinate coordinate1, Coordinate coordinate2, double range)
    {
        return CoordinateHelpers.IsWithinDistanceFrom(coordinate1, coordinate2, range);
    }
}

