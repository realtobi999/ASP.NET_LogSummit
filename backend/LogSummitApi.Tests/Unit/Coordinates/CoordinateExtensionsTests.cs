using LogSummitApi.Application.Core.Services.Summits.Coordinates;
using LogSummitApi.Domain.Core.Utilities;

namespace LogSummitApi.Tests.Unit.Coordinates;

public class CoordinateExtensionsTests
{
    [Fact]
    public void TotalDistance_ShouldReturnCorrectDistance()
    {
        // prepare
        var coordinates = new List<Coordinate>
        {
            new(37.7749, -122.4194, 10), // San Francisco
            new(34.0522, -118.2437, 20), // Los Angeles
            new(36.1699, -115.1398, 15)  // Las Vegas
        };

        double expectedDistance = CoordinateMath.Haversine(coordinates[0], coordinates[1]) + CoordinateMath.Haversine(coordinates[1], coordinates[2]);

        // act & assert
        double totalDistance = CoordinateExtensions.TotalDistance(coordinates);

        totalDistance.Should().BeApproximately(expectedDistance, 0.1);
    }

    [Fact]
    public void TotalDistance_ShouldReturnZeroForSingleCoordinate()
    {
        // prepare
        var coordinates = new List<Coordinate>
        {
            new(37.7749, -122.4194, 10) 
        };

        // act & assert
        double totalDistance = CoordinateExtensions.TotalDistance(coordinates);

        totalDistance.Should().Be(0.0); 
    }
}
