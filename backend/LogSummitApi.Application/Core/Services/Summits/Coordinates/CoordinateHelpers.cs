using LogSummitApi.Domain.Core.Utilities;

namespace LogSummitApi.Application.Core.Services.Summits.Coordinates;

public static class CoordinateHelpers
{
    public static bool AreWithinRange(Coordinate coordinate1, Coordinate coordinate2, double range)
    {
        if (range <= 0) throw new ArgumentException("AreWithingRange 'range' argument needs to be bigger than zero.");

        var distance = CoordinateMath.Haversine(coordinate1, coordinate2);

        return distance <= range;
    }

    public static (double gain, double loss) CalculateElevationChange(IEnumerable<Coordinate> coordinates, double distanceThreshold, double verticalThreshold)
    {
        var gain = 0.0;
        var loss = 0.0;

        for (int i = 0; i < coordinates.Count() - 1; i++)
        {
            var current = coordinates.ElementAt(i);
            var next = coordinates.ElementAt(i + 1);

            double distance = CoordinateMath.Haversine(current, next);

            if (distance >= distanceThreshold)
            {
                var elevationDifference = next.Elevation - current.Elevation;

                if (elevationDifference >= verticalThreshold)
                {
                    gain += elevationDifference;
                }
                else if (elevationDifference <= -verticalThreshold)
                {
                    loss += elevationDifference;
                }
            }
        }

        return (gain, Math.Abs(loss));
    }
}
